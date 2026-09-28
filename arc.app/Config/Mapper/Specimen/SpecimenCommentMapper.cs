using arc.app.Common;

namespace arc.app.Config.Mapper.Specimen
{
    internal class SpecimenCommentMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'specimencommentmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Comment', Value: 'Comment' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'CannedComment', Value: 'CannedComment' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'PrintOnReport', Value: 'PrintOnReport' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'Id', Value: '<:username:>' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'CommentType', Value: 'CommentType' }
                        ],
                        'Target': 
                            {
                                'SpecimenId':'<:1:>',
                                'CommentTypeId': '<:6:>',
                                'Comment':'<:2:>',
                                'CannedCommentId':'<:3:>',
                                'DisplayOnReport':'<:4:>',                            
                                'AddedBy':'<:5:>'
                            }
                     }";
        }
    }
}
