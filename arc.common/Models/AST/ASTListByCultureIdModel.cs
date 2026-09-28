namespace arc.common.Models.AST
{
    public class ASTListByCultureIdModel
    {
        public int Id { get; set; }
        public int CultureId { get; set; }
        public string Dosage { get; set;}
        public string Measurement { get; set; }
        public string DisplayOnReport { get; set; }
        public string Susceptibility { get; set; }
        public string AntibioticName { get; set; }
        public string TestMethod { get; set; }
        public int TestMethodId { get; set; }
        public string SpecialConsideration { get; set; }
        /// <summary>Special breakpoint consideration list item id; 0 for parent AST rows.</summary>
        public int SpecialConsiderationId { get; set; }
    }
}
