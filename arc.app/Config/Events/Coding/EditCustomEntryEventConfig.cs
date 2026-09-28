using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditCustomEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editCustomEntry', 
                        Description: '@CodEdi@',
                        EventType : 'special', 
                        Topic : 'Coding',
                        ValidationRules: [
                            { field: 'Description', rule: 'required', message: '@CodAA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'checkduplicatecustomentrycode', message: '@CodThiB@' }
                        ]
                    }";
        }
    }
}

