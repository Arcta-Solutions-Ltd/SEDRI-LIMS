namespace arc.data.model.Patient;

/// <summary>
/// Represents the fields in the request table in the database.
/// </summary>
public class RequestDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the patient table to the request table.
    /// </summary>
    [ImmutableOnUpdate]
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the admission table to the request table.
    /// </summary>
    [ImmutableOnUpdate]
    public int? AdmissionId { get; set; }

    /// <summary>
    /// Gets or sets the human readable request reference.
    /// </summary>
    [ImmutableOnUpdate]
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets or sets additional data for the request in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }
}
