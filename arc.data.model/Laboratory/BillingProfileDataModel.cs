namespace arc.data.model.Laboratory;

/// <summary>
/// Represents the fields in the billingprofile table in the database.
/// </summary>
public class BillingProfileDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the name of the billing profile.
    /// </summary>
    public required string ProfileName { get; set; }

    /// <summary>
    /// Gets or sets the test name for the billing profile.
    /// </summary>
    public required string TestName { get; set; }

    /// <summary>
    /// Gets or sets the test rule for the billing profile.
    /// </summary>
    public string? TestRule { get; set; }

    /// <summary>
    /// Gets or sets the specimen rule for the billing profile.
    /// </summary>
    public string? SpecimenRule { get; set; }

    /// <summary>
    /// Gets or sets the culture rule for the billing profile.
    /// </summary>
    public string? CultureRule { get; set; }

    /// <summary>
    /// Gets or sets the patient rule for the billing profile.
    /// </summary>
    public string? PatientRule { get; set; }

    /// <summary>
    /// Gets or sets additional data in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the laboratory table to the billingprofile table.
    /// </summary>
    public int? LaboratoryId { get; set; }
}
