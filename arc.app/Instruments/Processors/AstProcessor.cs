using arc.app.AST;
using arc.app.Coding;
using arc.app.Specimen;
using arc.common.Models;
using arc.common.Models.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Instruments.Processors
{
    internal class AstProcessor(IServiceProvider serviceProvider) : IRespond
    {
        private readonly ICultureRepository _cultureRepository = serviceProvider.GetService<ICultureRepository>();
        private readonly IInstrumentRequestHandler _instrumentRequestHandler = serviceProvider.GetService<IInstrumentRequestHandler>();
        private readonly IOrganismRepository _organismRepository = serviceProvider.GetService<IOrganismRepository>();
        private readonly IASTRepository _astRepository = serviceProvider.GetService<IASTRepository>();
        private readonly IAntibioticRepository _antibioticRepository = serviceProvider.GetService<IAntibioticRepository>();
        private readonly IInstrumentRepository _instrumentRepository = serviceProvider.GetService<IInstrumentRepository>();

        /// <inheritdoc />
        public async Task<bool> Run(ResponseModel response, TokenInfoModel token)
        {
            try
            {
                var errorMessage = new StringBuilder();
                if (string.IsNullOrWhiteSpace(response.Identifier))
                {
                    errorMessage.AppendLine("No barcode provided with this record");
                }

                // Find the culture to update based on the barcode

                var queryFilters = new QueryFilterConfig();
                queryFilters.AddString("AccessionNumber", response.AccessionNumber);
                queryFilters.AddString("CultureNumber", response?.CultureNumber);

                var culture = await _cultureRepository.GetCultureByAccessionNumberAndCultureNumberAsync(queryFilters);

                if (culture.Id == 0)
                {
                    errorMessage.AppendLine("Was not able to find the culture");
                }
                if (response.OrganismId > 0 && culture.SpecimenOrganismId > 0 && ! response.AllowIdOverwrite)
                {
                    errorMessage.AppendLine("An organism has already been identified for this culture");
                }
                response.CultureId = culture.Id;

                //TODO Check that the specimen has not been finalised - Overwrite growth

                //Get the organism id from the code

                if (response.OrganismList > 0)
                {
                    queryFilters.Clear();
                    queryFilters.AddInteger("codingid", response.OrganismList);
                    queryFilters.AddString("code", response.OrganismCode);
                    var organism = await _organismRepository.OrganismByCodeQueryAsync(queryFilters);

                    if (organism == null)
                    {
                        errorMessage.AppendLine("Was not able to find the organism");
                    }
                    response.OrganismId = organism == null ? 0 :organism.Id;

                    //Check to see whether there are already AST results for this organism

                    queryFilters.Clear();
                    queryFilters.AddInteger("cultureid", culture.Id);
                    var astResults = await _astRepository.GetAstListByCultureIdAsync(queryFilters);

                    if (astResults.Count() > 0 && !response.AllowAstOverwrite)
                    {
                        errorMessage.AppendLine("This culture already contains Ast results");
                    }

                    //Get the antibioticid for the AST entry

                    var errormessage = "";
                    var kept = new List<AntibioticResponseModel>();
                    foreach (var antibiotic in response.Antibiotics)
                    {
                        queryFilters.Clear();
                        queryFilters.AddString("code", antibiotic.Code);
                        queryFilters.AddInteger("codingid", response.AntibioticList);
                        var antibioticInDatabase = await _antibioticRepository.GetAntibioticEntryByCodeAsync(queryFilters);
                        if (antibioticInDatabase == null)
                        {
                            if (!response.IgnoreUnrecognisedAntibiotics)
                                errormessage = string.IsNullOrEmpty(errormessage) ? antibiotic.Code : errormessage + "," + antibiotic.Code;
                            continue;
                        }
                        antibiotic.AntibioticId = antibioticInDatabase.Id;
                        kept.Add(antibiotic);
                    }
                    response.Antibiotics = kept;

                    if (!string.IsNullOrEmpty(errormessage))
                    {
                        errorMessage.AppendLine("The following antibiotic codes could not be found: " + errormessage);
                    }

                    if (errorMessage.Length > 0) 
                    {
                        await MakeErrorAsync(response, errorMessage.ToString());
                    }
                }

                //Update the culture entry and AST results

                if (string.IsNullOrEmpty(errorMessage.ToString()))
                {
                    var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger(typeof(AstProcessor));
                    if (response.ResistanceMechanisms != null && logger != null)
                    {
                        logger.LogInformation(
                            "Instrument AST: applying resistance mechanisms replace CultureId={CultureId} Count={Count}",
                            response.CultureId,
                            response.ResistanceMechanisms.Count);
                    }

                    await _instrumentRepository.InstrumentCultureUpdateAsync(response);
                }
                return true;
            }
            catch (Exception ex)
            {
                await MakeErrorAsync(response, ex.Message);
                return false;
            }
        }

        private async Task MakeErrorAsync(ResponseModel response, string message)
        {
            var error = new InstrumentErrorModel
            {
                DirectionId = 19,
                Description = message,
                Message = JsonConvert.SerializeObject(response),
                InstrumentResultId = 11,
                ProfileName = response.ProfileName
            };
            await _instrumentRequestHandler.SaveErrorAsync(error);
        }

    }
}
