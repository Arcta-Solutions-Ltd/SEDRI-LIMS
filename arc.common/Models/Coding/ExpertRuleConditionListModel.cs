namespace arc.common.Models.Coding;

/// <summary>
/// List model for displaying expert rule conditions in the record view embedded list.
/// All list-item and FK fields are resolved to display text.
/// </summary>
public class ExpertRuleConditionListModel
{
    public int Id { get; set; }
    /// <summary>
    /// Antibiotic name or antibiotic group name, whichever is set.
    /// </summary>
    public string AntibioticDisplay { get; set; }
    public string TestMethodName { get; set; }
    public string SusceptibilityName { get; set; }
    public string SpecialConsiderationName { get; set; }
    public decimal? StartVal { get; set; }
    public decimal? EndVal { get; set; }
}
