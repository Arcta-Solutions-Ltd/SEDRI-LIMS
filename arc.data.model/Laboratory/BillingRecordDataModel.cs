namespace arc.data.model.Laboratory;

/// <summary>
/// Represents the billingrecord table.
/// </summary>
public class BillingRecordDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the laboratory this charge belongs to.
    /// </summary>
    public int LaboratoryId { get; set; }

    /// <summary>
    /// Gets or sets the patient identifier.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the specimen identifier.
    /// </summary>
    public int SpecimenId { get; set; }

    /// <summary>
    /// Gets or sets the culture identifier when applicable.
    /// </summary>
    public int? CultureId { get; set; }

    /// <summary>
    /// Gets or sets the direct test name being charged.
    /// </summary>
    public required string DirectTestName { get; set; }

    /// <summary>
    /// Gets or sets the test pattern identifier.
    /// </summary>
    public int TestPatternId { get; set; }

    /// <summary>
    /// Gets or sets the charge description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the total amount charged.
    /// </summary>
    public decimal TotalAmount { get; set; }
}
