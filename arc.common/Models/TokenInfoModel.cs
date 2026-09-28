namespace arc.common.Models;

/// <summary>
/// Represents the authentication and identity information associated with a user token.
/// </summary>
/// 
public class TokenInfoModel
{
    /// <summary>
    /// The unique identifier for the token or user session.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The username associated with the token.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The user's first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// The user's last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// The ID of the laboratory to which the user is assigned.
    /// </summary>
    public string LaboratoryId { get; set; }

    /// <summary>
    /// The ID of the user's organisation.
    /// </summary>
    public string OrganisationId { get; set; }

    /// <summary>
    /// The identifier representing the user's language preference.
    /// </summary>
    public string LanguageId { get; set; }

    /// <summary>
    /// The authentication method used (e.g., password, Azure AD, etc.).
    /// </summary>
    public string AuthMethod { get; set; }

    /// <summary>
    /// A delimited list of laboratory IDs the user is permitted to access.
    /// </summary>
    public string AllowedLaboratories { get; set; }

    /// <summary>
    /// A delimited list of organisation IDs the user is permitted to access.
    /// </summary>
    public string AllowedOrganisations { get; set; }

    /// <summary>
    /// A delimited list of tags added to the token.
    /// </summary>
    public string Tags { get; set; }
}
