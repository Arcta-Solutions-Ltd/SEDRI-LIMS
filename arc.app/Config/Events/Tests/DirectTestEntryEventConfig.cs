using arc.app.Common;

namespace arc.app.Config.Events
{
    public class DirectTestEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'DirectTestEntry',
                        Description: '@TesCar@',
                        Topic : 'Tests',
                        TableName: 'Tests'
                    }";
        }
    }
}
