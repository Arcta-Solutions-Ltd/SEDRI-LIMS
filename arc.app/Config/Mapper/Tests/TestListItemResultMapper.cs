using arc.app.Common;

namespace arc.app.Config.Mapper.Tests
{
    internal class TestListItemResultMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'testlistitemresultmapper', 'Type': 'Special'
                     }";
        }
    }
}
