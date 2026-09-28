namespace arc.common.Models.Specimen;

/// <summary>
/// Represents the data required to validate edits to a culture entity.
/// </summary>
public class EditCultureValidatorModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the culture record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated growth scenario.
    /// Optional; null means growth was omitted or not specified on edit.
    /// </summary>
    public int? GrowthId { get; set; }
}
