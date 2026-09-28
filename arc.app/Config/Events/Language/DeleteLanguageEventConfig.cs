using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteLanguageEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteLanguage', 
                        Description: '@LanDel@',
                        EventType : 'special',
                        Mapping: 'deletelanguagemapper',
                        Topic : 'Language', 
                        ValidationRules: [
                            { field: 'TranslationId', rule: 'required', message: '@LanAA@'},
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'languageusedinlaboratory', message: '@LanTraE@' },
                            { type: 'NoRecord', query: 'languageusedinorganisation', message: '@LanTraD@' }
                        ]
                    }";
        }
    }
}
