using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditLocationEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editLocation', 
                        Description: '@LocEdi@',
                        EventType : 'special', 
                        Topic : 'Location', 
                        TableName: 'Location',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@LocA@'},
                            { field: 'Name', rule: 'required', message: '@LocLocA@'}
                        ]
                    }";
        }

    }
}
