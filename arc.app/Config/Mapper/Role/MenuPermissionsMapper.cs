using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class MenuPermissionsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'menupermissionsmapper', 'Type': 'Special'
                     }";
        }
    }
}
