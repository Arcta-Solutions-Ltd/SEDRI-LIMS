using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddLanguageEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addLanguage', 
                        Description: '@LanAdd@',
                        EventType : 'specialadddata', 
                        Topic : 'Language', 
                        TableName: 'Language',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@LanTraC@'},
                            { field: 'SourceId', rule: 'required', message: '@LanA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'languageexists', message: '@LanThi@' }
                        ]
                    }";
        }
    }
}
