using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class DeleteLanguageMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'deletelanguagemapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'TranslationId', Value: 'TranslationId' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>'
                            }
                     }";
        }
    }
}
