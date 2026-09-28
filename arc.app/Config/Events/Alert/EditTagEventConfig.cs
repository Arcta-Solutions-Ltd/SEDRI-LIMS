using arc.app.Common;

namespace arc.app.Config.Events
{
    public class EditTagEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'edittag', 
                        Description: '@AleEdiA@',
                        EventType : 'special',
                        Mapping: 'edittagmapper', 
                        Topic : 'Tags',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'TagId', rule: 'required', message: '@AleThe@'},
                            { field: 'Name', rule: 'required', message: '@AleA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'tagexists', message: '@AleThi@' }
                        ]
                    }";
        }
    }
}
