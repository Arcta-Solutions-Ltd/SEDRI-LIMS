using arc.common.Models.Role;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.SidebarConfig;
using arc.domain.Security.Permission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Roles
{
    public interface IEventPermissionHandler
    {
        Task<string> GetEventPermissionsForRoleAsync(QueryFilterConfig queryFilters);
        Task<List<EventPermissionModel>> PopulateEventPermissionModelAsync(SidebarConfig sidebarConfig, Role roleDetails);
    }
}
