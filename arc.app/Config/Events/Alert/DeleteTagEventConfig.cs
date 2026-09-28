using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteTagEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deletetag', 
                        Description: '@AleDelB@',
                        EventType : 'deletedata', 
                        Topic : 'Tags',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@AleTheA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'taghaschildren', message: '@AleTagHasChildren@' }
                        ]
                    }";
        }
    }
}
