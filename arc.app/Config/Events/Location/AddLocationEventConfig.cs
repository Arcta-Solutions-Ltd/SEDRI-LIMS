using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddLocationEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addLocation', 
                        Description: '@LocAdd@',
                        EventType : 'specialadddata', 
                        Topic : 'Location', 
                        TableName: 'Location',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@LocLocA@'}
                        ]
                    }";
        }
    }
}
