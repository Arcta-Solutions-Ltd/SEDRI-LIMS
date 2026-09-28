namespace arc.identity;

/// <summary>
/// Represents the login command containing user credentials and an optional laboratory identifier
/// used for authentication purposes.
/// </summary>
public class LoginCommand
{
    /// <summary>
    /// Gets or sets the username provided for the login.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password provided for the login.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the laboratory identifier associated with the login, if applicable.
    /// </summary>
    public string LabId { get; set; }

    /// <summary>
    /// Gets or sets the organisation identifier associated with the login, if applicable.
    /// </summary>
    public string OrgId { get; set; }
}
