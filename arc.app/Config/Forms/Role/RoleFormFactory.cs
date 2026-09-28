using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class RoleFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addroleform" => new AddRoleFormConfig(),
                "cloneroleform" => new CloneRoleFormConfig(),
                "deleteroleform" => new DeleteRoleFormConfig(),
                "editroleform" => new EditRoleFormConfig(),
                "eventpermissions" => new EventPermissionsFormConfig(),
                "menupermissions" => new MenuPermissionsFormConfig(),
                _ => null,
            };
        }
    }
}
