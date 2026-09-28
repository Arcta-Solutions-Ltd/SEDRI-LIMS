namespace arc.data.model.Laboratory;

/// <summary>
/// Represents the fields in the contact table in the database.
/// </summary>
public class ContactDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the name of the contact.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the email address of the contact.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the telephone number of the contact.
    /// </summary>
    public string? Telephone { get; set; }

    /// <summary>
    /// Gets or sets the mobile number of the contact.
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// Gets or sets additional data in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }
}
