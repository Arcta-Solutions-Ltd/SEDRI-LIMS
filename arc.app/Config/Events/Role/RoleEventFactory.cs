using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class RoleEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addrole" => new AddRoleEventConfig(),
                "clonerole" => new CloneRoleEventConfig(),
                "deleterole" => new DeleteRoleEventConfig(),
                "editrole" => new EditRoleEventConfig(),
                "menupermissions" => new MenuPermissionEventConfig(),
                "eventpermissions" => new EventPermissionEventConfig(),
                _ => null,
            };
        }
    }
}
