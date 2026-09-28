namespace arc.data.model.User;

/// <summary>
/// Represents the fields in the role table in the database.
/// </summary>
internal class RoleDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the name of the role.
    /// </summary>
    public string Rolename { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the role.
    /// </summary>
    public string Roledescription { get; set; } = null!;

    /// <summary>
    /// Gets or sets whether the role is enabled.
    /// </summary>
    public string Enabled { get; set; } = null!;

    /// <summary>
    /// Gets or sets additional data in JSON format.
    /// </summary>
    [Jsonb]
    public string? Moredata { get; set; }

    /// <summary>
    /// Gets or sets the menu permissions for the role in JSON format.
    /// </summary>
    [Jsonb]
    public string? Menupermission { get; set; }

    /// <summary>
    /// Gets or sets the event permissions for the role in JSON format.
    /// </summary>
    [Jsonb]
    public string? Eventpermission { get; set; }
}
