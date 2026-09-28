namespace arc.common;

/// <summary>
/// Represents a model that contains a unique identifier.
/// </summary>
public class IdModel
{
    /// <summary>
    /// Gets or sets the unique identifier as a string.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Patient the created record was attached to. Only set by events that can create a patient inline, so
    /// a form offering to run again can attach the next record to the same patient.
    /// </summary>
    public string PatientId { get; set; }

    /// <summary>
    /// Admission the created record was attached to, when the event deals in admissions.
    /// </summary>
    public string AdmissionId { get; set; }

    /// <summary>
    /// Request the created record was attached to, when the event deals in requests.
    /// </summary>
    public string RequestId { get; set; }
}
