using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class RoleMapperFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addrolemapper" => new AddRoleMapper(),
                "menupermissionsmapper" => new MenuPermissionsMapper(),
                "eventpermissionsmapper" => new EventPermissionsMapper(),
                "usersinrolemapper" => new UsersInRoleMapper(),
                _ => null,
            };
        }
    }
}
