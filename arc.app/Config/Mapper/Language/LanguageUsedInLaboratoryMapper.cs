using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class LanguageUsedInLaboratoryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'languageusedinlaboratorymapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'TranslationId', Value: 'TranslationId' }
                        ],
                        'Target' : { 
                            'Name': 'languageusedinlaboratory', 
                            'Parameters': [ 
                                {'Key': 'LanguageId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
