using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditHostEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'edithost', 
                        Description: '@BreEdiC@',
                        EventType : 'editdata', 
                        Topic : 'Breakpoints',
                        Mapping: 'edithostmapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'HostId', rule: 'required', message: '@BreThe@'},
                            { field: 'Name', rule: 'required', message: '@BreAA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'hostexists', message: '@BreThiA@' }
                        ]
                    }";
        }
    }
}
