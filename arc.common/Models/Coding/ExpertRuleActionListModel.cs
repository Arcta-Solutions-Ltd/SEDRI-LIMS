namespace arc.common.Models.Coding;

/// <summary>
/// List model for displaying expert rule actions in the record view embedded list.
/// All list-item and FK fields are resolved to display text.
/// </summary>
public class ExpertRuleActionListModel
{
    public int Id { get; set; }

    /// <summary>
    /// Antibiotic name when the action targets a single antibiotic; empty when the action uses an antibiotic group only.
    /// </summary>
    public string AntibioticDisplay { get; set; }

    /// <summary>
    /// Antibiotic group name when the action targets a group; empty when the action uses a single antibiotic only.
    /// </summary>
    public string AntibioticGroupDisplay { get; set; }

    public string SusceptibilityName { get; set; }
    public string DisplayOnReport { get; set; }
}
