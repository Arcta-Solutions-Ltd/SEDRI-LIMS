namespace arc.common.Models.AST
{
    public class SusceptibilityModel
    {       
        public int? AntibioticId { get; set; }
        public string Dosage { get; set; }
        public int TestMethodId { get; set; }
        public int GuidelinesId { get; set; }
        public string DisplayOnReport { get; set; }
        public int SpecialConsiderationId { get; set; }
        public string SpecialConsideration { get; set; }
        public int SusceptibilityId { get; set; }
        public int CategoryId { get; set; }
        /// <summary>Winning <c>breakpoint.id</c> for this special consideration (and antibiotic/guideline/method).</summary>
        public int BreakpointId { get; set; }
    }
}
