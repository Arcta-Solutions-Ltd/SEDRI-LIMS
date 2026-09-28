using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EventPermissionsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'eventpermissionsmapper', 'Type': 'Special'
                     }";
        }
    }
}
