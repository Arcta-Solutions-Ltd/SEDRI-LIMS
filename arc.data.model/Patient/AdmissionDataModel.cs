namespace arc.data.model.Patient;

/// <summary>
/// Represents the fields in the admission table in the database.
/// </summary>
public class AdmissionDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the patient table to the admission table.
    /// </summary>
    [ImmutableOnUpdate]
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets additional data for the admission in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }
}
