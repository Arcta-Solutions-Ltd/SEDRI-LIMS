namespace arc.domain.Security.User;

/// <summary>
/// Represents a user entity with properties for identity, contact details, role information, and associated organization data.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the username used for user authentication.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the user's password.
    /// Note: It is recommended to store a password hash rather than the plain text.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the enabled status as a string.
    /// Typically, the value would be "Yes" if the user is enabled.
    /// </summary>
    public string Enabled { get; set; }

    /// <summary>
    /// Gets or sets the roles assigned to the user (e.g., as a comma-separated string).
    /// </summary>
    public string Roles { get; set; }

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the laboratory associated with the user.
    /// </summary>
    public string LaboratoryId { get; set; }

    /// <summary>
    /// Gets or sets the name of the laboratories associated with the user.
    /// </summary>
    public string Laboratories { get; set; }

    /// <summary>
    /// Gets or sets the name of the organisations associated with the user.
    /// </summary>
    public string Organisations { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the organization with which the user is associated.
    /// </summary>
    public string OrganisationId { get; set; }

    /// <summary>
    /// Determines whether the user is enabled.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the <see cref="Enabled"/> property, after trimming any trailing spaces, equals "Yes"; otherwise, <c>false</c>.
    /// </returns>
    public bool IsEnabled() => Enabled.TrimEnd() == "Yes";
}
