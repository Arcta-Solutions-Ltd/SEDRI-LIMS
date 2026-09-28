namespace arc.common.Models.Coding
{
    public class RuleActionGridModel
    {
        public int? ActionId { get; set; }
        public int ExpertRuleId { get; set; }
        public int? AntibioticId { get; set; }
        public int? AntibioticGroupId { get; set; }
        public int SusceptibilityId { get; set; }
        public string DisplayOnReport { get; set; }
    }
}
