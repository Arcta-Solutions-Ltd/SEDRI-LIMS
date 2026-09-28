using arc.app.Roles;
using arc.domain.Security.Permission;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Security;

/// <summary>
/// This class handles permission-related operations for users, such as retrieving and combining role permissions.
/// </summary>
public class PermissionHandler : IPermissionHandler
{
    private readonly IRoleRepository _roleRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionHandler"/> class with the specified role repository.
    /// </summary>
    /// <param name="roleRepository">The role repository used to retrieve roles for users.</param>
    public PermissionHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// Asynchronously retrieves and combines permissions for the specified user.
    /// </summary>
    /// <param name="userName">The name of the user to retrieve permissions for.</param>
    /// <returns>A <see cref="Role"/> object representing the combined permissions for the user.</returns>
    public async Task<Role> GetPermissionsAsync(string userName)
    {
        // Query to get all roles for the user
        var rolesResult = await _roleRepository.GetRolesForUserAsync(userName);
        var roles = rolesResult.ToList();

        // If multiple roles, combine permissions
        var roleWithCombinedPermissions = roles.FirstOrDefault();
        roleWithCombinedPermissions = roleWithCombinedPermissions == null ? new Role() : roleWithCombinedPermissions;
        bool include = false;

        foreach (var role in roles)
        {
            if (include)
            {
                roleWithCombinedPermissions.AddMenuPermissions(role.MenuPermission);
                roleWithCombinedPermissions.AddEventPermissions(role.EventPermission);
            }
            include = true;
        }

        // Add specific events manually if required

        if (roleWithCombinedPermissions.EventPermission.ToLower().Contains("culturetest") &&
            !roleWithCombinedPermissions.EventPermission.ToLower().Contains("culturetestselection"))
        {
            roleWithCombinedPermissions.AddEventPermissions("{'AllowedEvents': ['culturetestselection']}");
        }
        roleWithCombinedPermissions.AddEventPermissions("{'AllowedEvents': ['cultureprintselector']}");
            roleWithCombinedPermissions.AddEventPermissions("{'AllowedEvents': ['addrulecondition']}");

        // Grant savefilterpresetsevent to users who have Preferences menu access or preference event
        var hasPreferencesMenu = roleWithCombinedPermissions.MenuPermissionDetails?.AllowedSidebarItems?
            .Any(s => string.Equals(s, "preferences", StringComparison.OrdinalIgnoreCase)) == true;
        var hasPreferenceEvent = roleWithCombinedPermissions.EventPermission.ToLower().Contains("preference");
        if (hasPreferencesMenu || hasPreferenceEvent)
        {
            roleWithCombinedPermissions.AddEventPermissions("{'AllowedEvents': ['savefilterpresetsevent', 'savecolumnlayoutsevent']}");
        }

        roleWithCombinedPermissions.NormalizePermissionDetails();

        return roleWithCombinedPermissions;
    }
}
