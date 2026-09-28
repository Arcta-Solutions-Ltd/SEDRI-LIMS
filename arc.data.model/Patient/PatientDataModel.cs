namespace arc.data.model.Patient;

/// <summary>
/// Represents the fields in the patient table in the database.
/// </summary>
public class PatientDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the patient reference number.
    /// </summary>
    public string? PatientRef { get; set; }

    /// <summary>
    /// Gets or sets the patient's first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the patient's surname.
    /// </summary>
    public required string Surname { get; set; }

    /// <summary>
    /// Gets or sets the patient's age.
    /// </summary>
    public string? Age { get; set; }

    /// <summary>
    /// Gets or sets the patient's date of birth.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the patient's telephone number.
    /// </summary>
    public string? TelephoneNumber { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the patient table. This links to the state list in the listitem table.
    /// </summary>
    public int? StateId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the patient table. This links to the gender list in the listitem table.
    /// </summary>
    public int? GenderId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the location table to the patient table.
    /// </summary>
    public int? LocationId { get; set; }

    /// <summary>
    /// Gets or sets the first line of the patient's address.
    /// </summary>
    public string? AddressLine1 { get; set; }

    /// <summary>
    /// Gets or sets the second line of the patient's address.
    /// </summary>
    public string? AddressLine2 { get; set; }

    /// <summary>
    /// Gets or sets the patient's zip code.
    /// </summary>
    public string? ZipCode { get; set; }

    /// <summary>
    /// Gets or sets the patient's barcode.
    /// </summary>
    public string? Barcode { get; set; }

    /// <summary>
    /// Gets or sets additional data for the patient in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }
}
