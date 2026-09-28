namespace arc.data.model.Culture;

/// <summary>
/// Represents the fields in the culturealert table in the database.
/// </summary>
public class CultureAlertDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the culture table to the culturealert table.
    /// </summary>
    public int? CultureId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the alert table to the culturealert table.
    /// </summary>
    public int? AlertId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the alerttype table to the culturealert table.
    /// </summary>
    public int? AlertTypeId { get; set; }
}
