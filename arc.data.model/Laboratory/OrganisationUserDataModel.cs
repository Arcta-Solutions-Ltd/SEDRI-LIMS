namespace arc.data.model.Laboratory;

/// <summary>
/// Represents the fields in the organisationuser table in the database.
/// </summary>
public class OrganisationUserDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the organisation table to the organisationuser table.
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the users table to the organisationuser table.
    /// </summary>
    public int UserId { get; set; }
}
