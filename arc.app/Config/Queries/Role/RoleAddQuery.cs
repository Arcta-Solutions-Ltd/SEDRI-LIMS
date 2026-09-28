using arc.app.Common;

namespace arc.app.Config.Queries.Role
{
    internal class RoleAddQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'roleaddquery', 'Type': 'Special', Tablename: 'Role', Translate: true}";
        }
    }
}
