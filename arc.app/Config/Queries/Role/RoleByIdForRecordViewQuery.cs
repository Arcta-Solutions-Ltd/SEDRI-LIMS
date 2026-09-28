using arc.app.Common;

namespace arc.app.Config.Queries.Role
{
    internal class RoleByIdForRecordViewQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'rolebyidforrecordviewquery', 'Type': 'Special', Tablename: 'Role', Translate: true}";
        }
    }
}
