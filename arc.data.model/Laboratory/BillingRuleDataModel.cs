namespace arc.data.model.Laboratory;

/// <summary>
/// Represents the billingrule table.
/// </summary>
public class BillingRuleDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the display name of the billing rule.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the laboratory this rule belongs to.
    /// </summary>
    public int LaboratoryId { get; set; }
}
