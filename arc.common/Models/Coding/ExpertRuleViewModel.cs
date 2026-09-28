namespace arc.common.Models.Coding;

/// <summary>
/// View model for displaying expert rule details on the expert rule record view.
/// All list-item foreign keys are resolved to display text.
/// Multiselect fields (SpecimenTypesToInclude, SpecimenTypesToExclude) are comma-separated strings.
/// </summary>
public class ExpertRuleViewModel
{
    public int Id { get; set; }
    public string ExpertRuleName { get; set; }
    public string RuleText { get; set; }
    public string OrderName { get; set; }
    public string FamilyName { get; set; }
    public string OrganismName { get; set; }
    public string OrgGroupName { get; set; }
    public string Specification { get; set; }
    public string CombinationRule { get; set; }
    public string Enabled { get; set; }
    public string AlertOnRule { get; set; }
    public string TagId { get; set; }
    /// <summary>
    /// Comma-separated list of specimen type names to include.
    /// </summary>
    public string SpecimenTypesToInclude { get; set; }
    /// <summary>
    /// Comma-separated list of specimen type names to exclude.
    /// </summary>
    public string SpecimenTypesToExclude { get; set; }
}
