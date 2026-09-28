namespace arc.data.model.Patient;

/// <summary>
/// Represents the fields in the patientcomment table in the database.
/// </summary>
public class PatientCommentDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the patient table to the patientcomment table.
    /// </summary>
    public int? PatientId { get; set; }

    /// <summary>
    /// Gets or sets the comment text.
    /// </summary>
    public required string Comment { get; set; }
}
