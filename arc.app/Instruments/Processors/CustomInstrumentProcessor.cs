using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using arc.app.Common;
using arc.app.Exports;
using arc.app.Files;
using arc.app.Import;
using arc.app.Instruments.CustomImport;
using arc.common.Models;
using arc.common.Models.Instruments;
using arc.common.Models.Instruments.CustomImport;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace arc.app.Instruments.Processors
{
    /// <summary>
    /// Loads an individual inbound file for a Custom interface profile (InterfaceTypeId 10). Reverse-maps the uploaded
    /// JSON/XML file against the profile's linked export profile mapping, evaluates the profile's interface criteria (by
    /// id), and upserts patient/specimen/culture/direct-test/isolate-test/AST records inside a single transaction. Any
    /// failure is recorded as an instrument error with a meaningful (translatable) description. This replaces the legacy
    /// MTBPCR-only <see cref="DirectTestInstrumentProcessor"/> for the Custom interface type.
    /// </summary>
    internal class CustomInstrumentProcessor(IServiceProvider serviceProvider) : IRespond
    {
        /// <inheritdoc />
        public async Task<bool> Run(ResponseModel response, TokenInfoModel token)
        {
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(CustomInstrumentProcessor));
            var errorHandler = serviceProvider.GetRequiredService<IInstrumentRequestHandler>();
            var logWriter = serviceProvider.GetRequiredService<ILogWriter>();

            if (token == null)
            {
                logger.LogError("CustomInstrumentProcessor requires a token for the record save.");
                await RaiseErrorAsync(errorHandler, response, "@InsCusSave@", "No authentication context was supplied for the custom interface load.");
                return false;
            }

            if (response.SourceFileAttachmentIds == null || response.SourceFileAttachmentIds.Count == 0)
            {
                await RaiseErrorAsync(errorHandler, response, "@InsCusParse@", "The custom interface load requires an uploaded source file.");
                return false;
            }

            try
            {
                var singleInstrumentProfile = serviceProvider.GetRequiredService<ISingleInstrumentProfile>();
                var profile = await singleInstrumentProfile.GetAsync(response.ProfileName?.Trim());
                if (profile == null)
                {
                    await RaiseErrorAsync(errorHandler, response, "@InsCusNoExp@", $"No instrument profile found for '{response.ProfileName}'.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(profile.ExportProfileId))
                {
                    await RaiseErrorAsync(errorHandler, response, "@InsCusNoExp@", $"Custom profile '{response.ProfileName}' has no export profile configured.");
                    return false;
                }

                var fieldRepository = serviceProvider.GetRequiredService<IExportProfileFieldRepository>();
                var mappingRepository = serviceProvider.GetRequiredService<IExportProfileMappingRepository>();

                var queryFilters = new QueryFilterConfig();
                queryFilters.AddString("exportprofileid", profile.ExportProfileId.Trim());
                var fields = (await fieldRepository.GetByProfileIdAsync(queryFilters)).ToList();
                var mapping = await mappingRepository.GetByProfileIdAsync(queryFilters);

                if (fields.Count == 0 || mapping == null || string.IsNullOrWhiteSpace(mapping.Structure))
                {
                    await RaiseErrorAsync(errorHandler, response, "@InsCusNoExp@",
                        $"Export profile {profile.ExportProfileId} has no mapping/fields to reverse-map the inbound file.");
                    return false;
                }

                var fileHandler = serviceProvider.GetRequiredService<IFileHandler>();
                var attachmentId = response.SourceFileAttachmentIds[0];
                var read = await fileHandler.ReadByIdAsync(attachmentId);
                var text = Encoding.UTF8.GetString(read.Bytes);
                logWriter.LogInfo(
                    $"Custom import: profile='{response.ProfileName}', exportProfileId={profile.ExportProfileId}, fileAttachmentId={attachmentId}, format={mapping.Format}",
                    nameof(CustomInstrumentProcessor), nameof(Run));

                var parser = serviceProvider.GetRequiredService<ICustomImportRecordParser>();
                ImportRecord record;
                try
                {
                    record = parser.Parse(text, mapping, fields);
                }
                catch (FormatException fex)
                {
                    await RaiseErrorAsync(errorHandler, response, "@InsCusParse@", fex.Message);
                    return false;
                }

                var criteriaEvaluator = serviceProvider.GetRequiredService<IInterfaceCriteriaEvaluator>();
                if (!await criteriaEvaluator.IsMatchAsync(profile, record))
                {
                    logWriter.LogInfo(
                        $"Custom import: record did not match interface criteria for profile '{response.ProfileName}'; nothing loaded",
                        nameof(CustomInstrumentProcessor), nameof(Run));
                    return true;
                }

                var uniqueReferenceResolver = serviceProvider.GetRequiredService<IUniqueReferenceResolver>();
                var references = uniqueReferenceResolver.Resolve(mapping.Structure, fields);

                var handler = serviceProvider.GetRequiredService<ICustomImportHandler>();
                var result = await handler.ImportAsync(record, references, profile, token);

                logWriter.LogInfo(
                    $"Custom import complete: profile='{response.ProfileName}', created={result.CreatedCount}, updated={result.UpdatedCount}, patientId={result.PatientId}, specimens=[{string.Join(",", result.SpecimenIds)}]",
                    nameof(CustomInstrumentProcessor), nameof(Run));
                return true;
            }
            catch (CustomImportException cex)
            {
                logger.LogError(cex, "CustomInstrumentProcessor business rule failure.");
                await RaiseErrorAsync(errorHandler, response, cex.ErrorTag, cex.Message);
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "CustomInstrumentProcessor failed.");
                await RaiseErrorAsync(errorHandler, response, "@InsCusSave@", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Records a failure as an instrument error. The language tag is stored in the (translatable) error text column
        /// and a JSON detail payload in the message column.
        /// </summary>
        private static async Task RaiseErrorAsync(IInstrumentRequestHandler errorHandler, ResponseModel response, string errorTag, string detail)
        {
            var error = new InstrumentErrorModel
            {
                ProfileName = response?.ProfileName ?? string.Empty,
                DirectionId = 19,
                Description = errorTag,
                Message = JsonConvert.SerializeObject(new { detail, profile = response?.ProfileName }),
                ErrorStatusId = 11
            };
            await errorHandler.SaveErrorAsync(error);
        }
    }
}
