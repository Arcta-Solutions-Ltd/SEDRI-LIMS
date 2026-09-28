using arc.app.Config.Sidebar;
using arc.common.Constants;
using arc.common.Models;
using arc.common.Models.Role;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.SidebarConfig;
using arc.domain.Security.Permission;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Roles;

public class MenuPermissionHandler : IMenuPermissionHandler
{
    private readonly IRoleRepository _roleRepository;

    public MenuPermissionHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<string> GetMenuPermissionsForRoleAsync(QueryFilterConfig queryFilters)
    {
        var roleId = queryFilters.GetIntegerValue("id");
        var roleDetails = await _roleRepository.GetRoleByIdAsync(roleId);

        var sidebarConfig = new SidebarSetup().GetLeftSidebarConfig();

        var menuListToReturn = PopulatePermissionModelList(sidebarConfig, roleDetails);

        var craftedModels = new List<CraftedModel>
        {
            new() { Name = "menupermission", Contents = JsonConvert.SerializeObject(menuListToReturn.OrderBy(o => o.Name)) }
        };
        var returnModel = new JustCraftedPages { Crafted = craftedModels };
        return JsonConvert.SerializeObject(returnModel);
    }

    /// <summary>
    /// Builds the menu permission toggle list for a role from sidebar config and stored grants.
    /// </summary>
    /// <param name="sidebarConfig">The sidebar configuration containing all menu items.</param>
    /// <param name="roleDetails">The role whose menu permissions are being loaded.</param>
    /// <returns>A list of crafted selection models for each sidebar menu item.</returns>
    public List<CraftedSelectionsModel> PopulatePermissionModelList(SidebarConfig sidebarConfig, Role roleDetails)
    {
        var returnConfig = sidebarConfig.GetListOfViews().Select(s => new { s.Name, s.Key });

        var menuListToReturn = new List<CraftedSelectionsModel>();
        foreach (var config in returnConfig)
        {
            string[] found = [];
            if (roleDetails.MenuPermissionDetails != null && roleDetails.MenuPermissionDetails.AllowedSidebarItems != null)
            {
                found = System.Array.FindAll(roleDetails.MenuPermissionDetails.AllowedSidebarItems, r => r == config.Key);
            }

            var isMandatory = MenuPermissionKeys.MandatorySidebarItems.Any(k =>
                string.Equals(k, config.Key, StringComparison.OrdinalIgnoreCase));
            var allowed = isMandatory || found.Length > 0 ? "Yes" : "No";

            var newItem = new CraftedSelectionsModel { Key = config.Key, Name = config.Name, Allowed = allowed };
            menuListToReturn.Add(newItem);
        }

        return menuListToReturn;
    }
}
