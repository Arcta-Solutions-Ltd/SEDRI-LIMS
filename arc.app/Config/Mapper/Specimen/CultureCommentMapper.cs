using arc.app.Common;

namespace arc.app.Config.Mapper.Specimen
{
    internal class CultureCommentMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'culturecommentmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'specimenId', Value: 'specimenId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'CommentType', Value: 'CommentType' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'Comment', Value: 'Comment' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'CannedComment', Value: 'CannedComment' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'PrintOnReport', Value: 'PrintOnReport' }, 
                            { Key: '<:7:>', Type: 'Mapping', Source: 'Id', Value: '<:username:>' },                          
                        ],
                        'Target': 
                            {
                                'CultureId':'<:1:>',
                                'SpecimenId':'<:2:>',
                                'CommentTypeId':'<:3:>',
                                'Comment':'<:4:>',
                                'CannedCommentId':'<:5:>',
                                'DisplayOnReport':'<:6:>',
                                'AddedBy':'<:7:>'
                            }
                     }";
        }
    }
}
