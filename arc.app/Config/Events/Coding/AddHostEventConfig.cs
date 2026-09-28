using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddHostEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addhost', 
                        Description: '@BreAddC@',
                        EventType : 'adddata', 
                        Topic : 'Breakpoints',
                        Mapping: 'addhostmapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@ConAB@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'hostexists', message: '@BreThiA@' }
                        ]
                    }";
        }
    }
}
