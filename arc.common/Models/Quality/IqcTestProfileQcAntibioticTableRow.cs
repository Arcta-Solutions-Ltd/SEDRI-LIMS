namespace arc.common.Models.QualityAssurance
{
    public class IqcTestProfileQcAntibioticTableRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Enabled { get; set; }
        public string Content { get; set; }
        public decimal RangeLower { get; set; }
        public decimal RangeUpper { get; set; }
        public decimal TargetLower { get; set; }
        public decimal TargetUpper { get; set; }
    }
}
