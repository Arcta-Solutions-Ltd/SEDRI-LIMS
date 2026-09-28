using System.Collections.Generic;

namespace arc.common.Models.QualityAssurance
{
    public class EditIqcTestModel
    {
        public int Id { get; set; }
        public List<QcOrganismWithIqcResultsModel> QcOrganismWithIqcResults { get; set; } = new List<QcOrganismWithIqcResultsModel>();
    }

    public class QcOrganismWithIqcResultsModel
    {
        public int QcOrganismId { get; set; }
        public string QcOrganismName { get; set; }
        public string PrimaryStrain { get; set; }
        public string StandardsBody { get; set; }
        public List<IqcResultModel> IqcResults { get; set; } = new List<IqcResultModel>();
    }

    public class IqcResultModel
    {
        public string TestMethod { get; set; }
        public int IqcResultId { get; set; }
        public string AntibioticName { get; set; }
        public decimal? ResultValue { get; set; }
        public decimal? ExpectedLowerValue { get; set; }
        public decimal? ExpectedUpperValue { get; set; }
    }
}
