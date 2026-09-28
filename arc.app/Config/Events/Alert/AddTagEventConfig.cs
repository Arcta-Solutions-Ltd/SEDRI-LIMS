using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddTagEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addtag', 
                        Description: '@AleAddB@',
                        EventType : 'specialadddata', 
                        Topic : 'Tags',
                        Mapping: 'addtagmapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@AleA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'tagexists', message: '@AleThi@' }
                        ]
                    }";
        }
    }
}
