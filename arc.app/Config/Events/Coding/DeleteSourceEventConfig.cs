using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteSourceEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deletesource', 
                        Description: '@CodDelF@',
                        EventType : 'deletedata', 
                        Topic: 'Coding',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@CodAE@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'checkwhetherlistitemisfixed', message: '@CodThiD@' },
                            { type: 'NoRecord', query: 'checkwhethersourcecontainsbreakpoints', message: '@CodThiE@' },
                            { type: 'NoRecord', query: 'checkwhethersourcecontainsexpertrules', message: '@CodThiEr@' }
                        ]
                    }";
        }
    }
}
