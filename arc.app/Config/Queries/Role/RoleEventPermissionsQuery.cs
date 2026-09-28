using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class RoleEventPermissionsQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'roleeventpermissions', 'Type': 'Special', Tablename: 'Role', Translate: true}";
        }
    }
}
