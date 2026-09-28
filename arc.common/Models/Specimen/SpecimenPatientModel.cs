namespace arc.common.Models.Specimen;

/// <summary>
/// Represents a patient-associated specimen within a medical or laboratory system.
/// </summary>
public class SpecimenPatientModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the specimen.
    /// </summary>
    public int SpecimenId { get; set; }

    /// <summary>
    /// Gets or sets the accession number assigned to the specimen, typically used for tracking in laboratory workflows.
    /// </summary>
    public string AccessionNumber { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the patient associated with this specimen.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the current processing state of the specimen (e.g., collected, received, processed).
    /// </summary>
    public int StateId { get; set; }
}
