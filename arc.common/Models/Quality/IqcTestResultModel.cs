namespace arc.common.Models.QualityAssurance
{
    public class IqcTestResultModel
    {
        public int Id { get; set; }
        public string OrganismName { get; set; }
        public string AntibioticName { get; set; }
        public float Value { get; set; }
        public string TestMethod { get; set; }
        public float MicRangeLower { get; set; }
        public float MicRangeUpper { get; set; }
        public float DiskRangeLower { get; set; }
        public float DiskRangeUpper { get; set; }
    }
}
