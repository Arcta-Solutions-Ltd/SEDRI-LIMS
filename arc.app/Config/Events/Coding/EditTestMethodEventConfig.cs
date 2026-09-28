using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditTestMethodEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'edittestmethod', 
                        Description: '@BreEdiB@',
                        EventType : 'editdata', 
                        Topic : 'Breakpoints',
                        Mapping: 'edittestmethodmapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'TestMethodId', rule: 'required', message: '@BreTheA@'},
                            { field: 'Name', rule: 'required', message: '@BreAB@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'testmethodexists', message: '@BreThi@' }
                        ]
                    }";
        }
    }
}
