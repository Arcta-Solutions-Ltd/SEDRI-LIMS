namespace arc.data.model.Specimen;

/// <summary>
/// Represents the fields in the specimenalert table in the database.
/// </summary>
public class SpecimenAlertDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the specimen table to the specimenalert table.
    /// </summary>
    public int? SpecimenId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the culture table to the specimenalert table.
    /// </summary>
    public int? CultureId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the alert table to the specimenalert table.
    /// </summary>
    public int? AlertId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the alerttype table to the specimenalert table.
    /// </summary>
    public int? AlertTypeId { get; set; }
}
