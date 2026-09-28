using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Exports.ExportFieldProcessors
{
    internal class WhonetProcessor
    {
        private IWhonetAntibioticColumns _whonetAntibioticColumns;
        private List<string> _antibioticList;

        public void Initialise(IWhonetAntibioticColumns whonetAntibioticColumns, List<WhonetAntibiotic> astResults, QueryFilterConfig queryFilters, TokenInfoModel token)
        {
            _whonetAntibioticColumns = whonetAntibioticColumns;

            var cultureId = 0;
            var cultureResults = new List<WhonetAntibiotic>();

            foreach (var result in astResults)
            {
                if (cultureId != result.CultureId)
                {
                    if (cultureId > 0 && cultureResults.Count > 0)
                    {
                        _whonetAntibioticColumns.AddOrganismRecord(cultureId, cultureResults);
                    }
                    cultureResults.Clear();
                }
                cultureResults.Add(result);
                cultureId = result.CultureId;
            }

            if (cultureId > 0 && cultureResults.Count > 0)
            {
                _whonetAntibioticColumns.AddOrganismRecord(cultureId, cultureResults);
            }

            _antibioticList = _whonetAntibioticColumns.GetAntibioticList();
            _antibioticList = _antibioticList.Distinct().OrderBy(o => o).ToList();
        }

        public string GetHeader()
        {
            var header = "";
            foreach (var antibiotic in _antibioticList)
            {
                header += header == "" ? antibiotic : "|" + antibiotic;
            }

            return header;
        }

        public string GetLine(int cultureId, string useQualitativeValues)
        {
            var recordList = _whonetAntibioticColumns.GetAntibioticForCulture(cultureId);
            string newLine = null;
            foreach (var antibiotic in _antibioticList)
            {
                WNExportList foundAntibiotic = null;
                if (recordList != null)
                {
                    foundAntibiotic = recordList.FirstOrDefault(a => a.ColumnCode == antibiotic);
                }
                if (foundAntibiotic == null)
                {
                    newLine = newLine == null ? "" : newLine + "|";
                }
                else
                {
                    if(useQualitativeValues == "Yes")
                    {
                        newLine = newLine == null ? foundAntibiotic.QualitativeValue : newLine + "|" + foundAntibiotic.QualitativeValue;
                    }
                    else
                    {
                        newLine = newLine == null ? foundAntibiotic.Measurement : newLine + "|" + foundAntibiotic.Measurement;
                    }
                }
            }
            return newLine;
        }
    }
}
