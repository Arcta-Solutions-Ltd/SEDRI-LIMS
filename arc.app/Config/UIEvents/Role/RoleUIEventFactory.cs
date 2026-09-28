using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class RoleUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addrole" => new AddRoleUIEventConfig(),
                "clonerole" => new CloneRoleUIEventConfig(),
                "deleterole" => new DeleteRoleUIEventConfig(),
                "editrole" => new EditRoleUIEventConfig(),
                "eventpermissions" => new EventPermissionsUIEventConfig(),
                "menupermissions" => new MenuPermissionsUIEventConfig(),
                "viewrolerecord" => new ViewRoleRecordUIEventConfig(),
                _ => null,
            };
        }
    }
}
