using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class CultureCommentFormQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'culturecommentformquerymapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'specimenid', Value: 'specimenid' },
                        ],
                        'Target':
                            {
                                'specimenid':'<:1:>'
                            }
                     }";
        }
    }
}

