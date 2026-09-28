using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddCustomEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addCustomEntry',
                        Description: '@CodAddA@',
                        EventType : 'specialadddata',
                        Topic : 'Coding',
                        ValidationRules: [
                            { field: 'Description', rule: 'required', message: '@CodAA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'checkduplicatecustomentry', message: '@CodThiB@' }
                        ]
                    }";
        }
    }
}
