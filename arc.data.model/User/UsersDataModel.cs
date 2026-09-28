namespace arc.data.model.User;

/// <summary>
/// Represents the fields in the users table in the database.
/// </summary>
internal class UsersDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets whether the user is enabled.
    /// </summary>
    public string Enabled { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    public string? Firstname { get; set; }

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    public string? Lastname { get; set; }

    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's username.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the organisation table to the users table.
    /// </summary>
    public int? Organisationid { get; set; }

    /// <summary>
    /// Gets or sets additional data in JSON format.
    /// </summary>
    [Jsonb]
    public string? Moredata { get; set; }
}
