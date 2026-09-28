using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddSourceEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addSource', 
                        Description: '@CodAddG@',
                        EventType : 'adddata', 
                        Mapping: 'addsourcemapper',
                        Topic: 'Coding',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@CodAD@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'sourcelistitemexists', message: '@CodThiC@' }
                        ]
                    }";
        }
    }
}
