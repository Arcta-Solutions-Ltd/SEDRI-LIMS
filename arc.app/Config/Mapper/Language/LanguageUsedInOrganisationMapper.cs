using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class LanguageUsedInOrganisationMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'languageusedinorganisationmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'TranslationId', Value: 'TranslationId' }
                        ],
                        'Target' : { 
                            'Name': 'languageusedinorganisation', 
                            'Parameters': [ 
                                {'Key': 'LanguageId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
