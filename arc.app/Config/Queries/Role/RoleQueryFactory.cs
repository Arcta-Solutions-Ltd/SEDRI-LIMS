using arc.app.Common;
using arc.app.Config.Queries.Role;

namespace arc.app.Config.Queries
{
    internal class RoleQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "roleaddquery" => new RoleAddQuery(),
                "rolebyid" => new RoleByIdQuery(),
                "rolebyidforeditrole" => new RoleByIdForEditRoleQuery(),
                "rolebyidforrecordviewquery" => new RoleByIdForRecordViewQuery(),
                "roleeventpermissions" => new RoleEventPermissionsQuery(),
                "rolelist" => new RoleListQuery(),
                "rolemenupermissions" => new RoleMenuPermissionsQuery(),
                "rolenameexists" => new RoleNameExistsQuery(),
                "singleroleforrolelist" => new SingleRoleForRoleListQuery(),
                "usersinrolecountquery" => new UsersInRoleCountQuery(),
                _ => null,
            };
        }
    }
}
