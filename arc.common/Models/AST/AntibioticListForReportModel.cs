namespace arc.common.Models.AST
{
    public class AntibioticListForReportModel
    {
        public int Id { get; set; }
        public string Antibiotic { get; set; }
        public string AntibioticCode { get; set; }
        public string Susceptibility { get; set; }
        public string DisplayOnReport { get; set; }
        public string Measurement { get; set; }
        public int Dosage { get; set; }
        public int TestMethodId { get; set; }
        public int GuidelinesId { get; set; }
        public int ExpertRuleId { get; set; }
        public string SpecialConsideration { get; set; }
        public string SpecialDisplayOnReport { get; set; }
        /// <summary>Special breakpoint consideration list item id; 0 or 973 for parent AST rows.</summary>
        public int SpecialConsiderationId { get; set; }
    }
}
