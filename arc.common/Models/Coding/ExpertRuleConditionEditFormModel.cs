using Newtonsoft.Json;

namespace arc.common.Models.Coding;

/// <summary>
/// Form payload and initial-query result for add/edit expert rule condition from the record view.
/// </summary>
public class ExpertRuleConditionEditFormModel
{
    /// <summary>
    /// Gets or sets the condition row id (edit) or expert rule id (add).
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
    /// Gets or sets the test method list item id.
    /// </summary>
    [JsonProperty("testmethodid")]
    public int? TestMethodId { get; set; }

    /// <summary>
    /// Gets or sets the special consideration list item id.
    /// </summary>
    public int? SpecialConsiderationId { get; set; }

    /// <summary>
    /// Gets or sets the optional measurement range start (null when unset).
    /// </summary>
    [JsonProperty("startval")]
    public decimal? StartVal { get; set; }

    /// <summary>
    /// Gets or sets the optional measurement range end (null when unset).
    /// </summary>
    [JsonProperty("endval")]
    public decimal? EndVal { get; set; }
}
