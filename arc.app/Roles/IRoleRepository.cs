using arc.domain.Configuration.ListsConfig;
using arc.domain.Security.Permission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Roles;

/// <summary>
/// Defines a repository interface for managing roles.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Retrieves roles associated with a specific user.
    /// </summary>
    /// <param name="user">The username or user identifier.</param>
    /// <returns>A collection of roles assigned to the user.</returns>
    Task<IEnumerable<Role>> GetRolesForUserAsync(string user);

    /// <summary>
    /// Retrieves a list of roles with their configuration options.
    /// </summary>
    /// <returns>A collection of role configuration options.</returns>
    Task<IEnumerable<OptionsConfig>> GetRolesForListAsync();

    /// <summary>
    /// Retrieves a specific role by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the role.</param>
    /// <returns>The role corresponding to the given ID.</returns>
    Task<Role> GetRoleByIdAsync(int id);

    /// <summary>
    /// Deletes a role from the system using its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the role to be deleted.</param>
    Task DeleteRoleAsync(int id);

    /// <summary>
    /// Creates a copy of an existing role with a new name and description.
    /// </summary>
    /// <param name="id">The ID of the role to clone.</param>
    /// <param name="roleName">The name of the new role.</param>
    /// <param name="roleDescription">A description of the new role.</param>
    /// <returns>The ID of the newly created role.</returns>
    Task<int> CloneRoleAsync(int id, string roleName, string roleDescription);
}
