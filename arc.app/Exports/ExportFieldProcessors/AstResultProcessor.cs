using arc.common.Models;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Exports.ExportFieldProcessors;
internal class AstResultProcessor
{
    private List<WhonetAntibiotic> _astResults;

    public async Task InitialiseAsync(IExportRepository exportRepository, QueryFilterConfig queryFilters, TokenInfoModel token, List<ExportProfileFieldModel> exportProfileFields, IJsonReplacer jsonReplacer)
    {
        var containsWhoNet = exportProfileFields.Any(p => p.FieldName.ToLower() == "whonetantibiotic");
        var containsAST = exportProfileFields.Any(p => p.FieldName.ToLower() == "qualitativeantibioticsusceptibility" || p.FieldName.ToLower() == "antibioticmeasurement");

        if (containsWhoNet)
        {
            _astResults = await exportRepository.AstExportAsync(queryFilters, token);
        }
        else
        {
            if (containsAST)
            {
                var antibiotics = "";
                foreach (var profileField in exportProfileFields)
                {
                    var val = jsonReplacer.GetValueInJsonString(profileField.MoreData, "Antibiotic");
                    if (val != null)
                    {
                        antibiotics += antibiotics == "" ? val : ", " + val;
                    }
                }
                queryFilters.AddString("antibiotics", antibiotics);
                _astResults = await exportRepository.AstExportAsync(queryFilters, token);
            }
        }
    }

    public List<WhonetAntibiotic> GetASTResultsList(string antibioticId = "")
    {
        var res = _astResults;
        if(Int32.TryParse(antibioticId, out var idAsInt))
        {
            res = _astResults.Where(p => p.AntibioticId == idAsInt).ToList();
        }
        return res;
    }
}
