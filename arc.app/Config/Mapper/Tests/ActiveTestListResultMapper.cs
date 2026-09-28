using arc.app.Common;

namespace arc.app.Config.Mapper.Tests
{
    internal class ActiveTestListResultMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'activetestlistresultmapper', 'Type': 'Special'
                     }";
        }
    }
}
