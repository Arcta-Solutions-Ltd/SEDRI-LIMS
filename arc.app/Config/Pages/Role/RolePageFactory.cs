using arc.app.Common;
using arc.app.Config.Pages.Role;

namespace arc.app.Config.Pages
{
    internal class RolePageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "clonerolepage" => new CloneRolePageConfig(),
                "deleterolepage" => new DeleteRolePageConfig(),
                "eventpermission" => new EventPermissionsPageConfig(),
                "menupermission" => new MenuPermissionsPageConfig(),
                "roleadddetails" => new AddRoleDetailsPageConfig(),
                "roleeditdetails" => new EditRoleDetailsPageConfig(),
                _ => null,
            };
        }
    }
}
