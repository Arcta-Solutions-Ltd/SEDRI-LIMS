namespace arc.data.model.Laboratory;

/// <summary>
/// Represents the fields in the organisation table in the database.
/// </summary>
public class OrganisationDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the name of the organisation.
    /// </summary>
    public required string OrganisationName { get; set; }

    /// <summary>
    /// Gets or sets the fully qualified name of the organisation.
    /// </summary>
    public string? FullyQualifiedName { get; set; }

    /// <summary>
    /// Gets or sets the code for the organisation.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the organisation table to itself (parent organisation).
    /// </summary>
    public int? ParentOrganisationId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the language table to the organisation table.
    /// </summary>
    public int? LanguageId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the location table to the organisation table.
    /// </summary>
    public int? LocationId { get; set; }

    /// <summary>
    /// Gets or sets additional data in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }

    /// <summary>
    /// Gets or sets whether the organisation is enabled.
    /// </summary>
    public required string Enabled { get; set; }
}
