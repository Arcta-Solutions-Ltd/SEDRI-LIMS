using Newtonsoft.Json;

namespace arc.common.Models.Coding;

/// <summary>
/// Form payload and initial-query result for add/edit expert rule action from the record view.
/// </summary>
public class ExpertRuleActionEditFormModel
{
    /// <summary>
    /// Gets or sets the action row id (edit) or expert rule id (add).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the antibiotic id.
    /// </summary>
    [JsonProperty("antibioticid")]
    public int? AntibioticId { get; set; }

    /// <summary>
    /// Gets or sets the antibiotic group id.
    /// </summary>
    [JsonProperty("antibioticgroupid")]
    public int? AntibioticGroupId { get; set; }

    /// <summary>
    /// Gets or sets the susceptibility list item id.
    /// </summary>
    public int? SusceptibilityId { get; set; }

    /// <summary>
    /// Gets or sets whether the result should appear on the report.
    /// </summary>
    public string DisplayOnReport { get; set; }
}
