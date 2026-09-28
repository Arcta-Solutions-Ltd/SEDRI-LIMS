using arc.common.Models.Export;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Exports
{
    public class WhonetAntibioticColumns : IWhonetAntibioticColumns
    {
        private readonly List<WNHeader> _recordList = [];
        private readonly List<string> _antibioticList = [];

        public void AddOrganismRecord(int cultureId, List<WhonetAntibiotic> antibioticRows)
        {
            var newHeader = new WNHeader { CultureId = cultureId };

            foreach (var row in antibioticRows)
            {
                // Create organism record

                if (row.Measurement != null)
                {
                    var newOrganismRecord = new WNExportList
                    {
                        ColumnCode = FormatWHONETAntibioticCode(row),
                        Measurement = row.Measurement.Contains("-1") ? "" : row.Measurement,
                        QualitativeValue = row.Susceptibility
                    };

                    newHeader.Antibiotics.Add(newOrganismRecord);

                    var foundAntibiotic = _antibioticList.Where(r => r == newOrganismRecord.ColumnCode).ToList();
                    if (foundAntibiotic.Count() == 0 && newOrganismRecord.Measurement != "")
                    {
                        _antibioticList.Add(newOrganismRecord.ColumnCode);
                    }
                }
            }
            _recordList.Add(newHeader);
        }

        public List<string> GetAntibioticList() { return _antibioticList;}

        public List<WNExportList> GetAntibioticForCulture(int cultureId)
        {
            var cultureInfo = _recordList.FirstOrDefault(l => l.CultureId == cultureId);

            if (cultureInfo == null)
            {
                return null;
            } else
            {
                return cultureInfo.Antibiotics;
            }
        }

        private string FormatWHONETAntibioticCode(WhonetAntibiotic antibiotic)
        {

            var antibioticCode = antibiotic.AntibioticCode;

            var guidelinesCode = "N"; 
            if (antibiotic.GuidelinesId > 0)
            {
                guidelinesCode = antibiotic.GuidelinesId == 971 ? "N" : antibiotic.GuidelinesId == 972 ? "E" : ""; 
            }
            var testMethodCode = antibiotic.TestMethodId == 681 ? "D" : antibiotic.TestMethodId == 680 ? "M" : ""; 
            var dosageRaw = antibiotic.Dosage.ToString();
            var dosage = (testMethodCode == "M" || dosageRaw == null || dosageRaw == "") ? "" : dosageRaw;
            var antibioticWHONETCode = antibioticCode + '_' + guidelinesCode + testMethodCode + dosage;

            if (antibioticWHONETCode == "SXT_ND25") { antibioticWHONETCode = "SXT_ND1_2"; }

            return antibioticWHONETCode;
        }
    }
}

