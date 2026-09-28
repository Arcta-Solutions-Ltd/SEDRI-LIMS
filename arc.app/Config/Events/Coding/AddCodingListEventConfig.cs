using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddCodingListEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addCodingList', 
                        Description: '@CodAddE@',
                        EventType : 'adddata', 
                        Mapping: 'addcodinglistmapper',
                        Topic: 'Coding',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@CodA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'codinglistitemexists', message: '@CodThi@' }
                        ]
                    }";
        }
    }
}
