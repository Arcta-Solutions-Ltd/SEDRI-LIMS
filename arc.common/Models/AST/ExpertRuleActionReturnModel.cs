namespace arc.common.Models.AST
{
    public class ExpertRuleActionReturnModel
    {
        public int Id { get; set; }
        public int ExpertRuleId { get; set; }
        public string ExpertRuleName { get; set; }
        public string RuleText { get; set; }
        public int AntibioticId { get; set; }
        /// <summary>
        /// When set, the action targets an antibiotic group instead of a single antibiotic.
        /// </summary>
        public int? AntibioticGroupId { get; set; }
        public int SourceId { get; set; }
        public int SusceptibilityId { get; set; }
        /// <summary>
        /// Gets or sets whether the result should be displayed on the report ('Yes' or 'No').
        /// </summary>
        public string DisplayOnReport { get; set; }
    }
}