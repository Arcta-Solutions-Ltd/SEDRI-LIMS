namespace arc.data.model.User;

/// <summary>
/// Represents the fields in the userrole table in the database.
/// </summary>
public class UserRoleDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the users table to the userrole table.
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the role table to the userrole table.
    /// </summary>
    public int? RoleId { get; set; }
}
