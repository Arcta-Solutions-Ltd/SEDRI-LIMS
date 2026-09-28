using arc.app.Common;

namespace arc.app.Config.Mapper.Specimen
{
    internal class EditCommentEventMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'editcommenteventmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'CommentTypeId', Value: 'CommentTypeId' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Comment', Value: 'Comment' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'PrintOnReport', Value: 'PrintOnReport' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'CannedComment', Value: 'CannedComment' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'Id', Value: '<:username:>' }
                        ],
                        'Target': 
                            {
                                'CommentType':'<:1:>',
                                'Comment':'<:2:>',
                                'DisplayOnReport':'<:3:>',
                                'Id':'<:4:>',
                                'CannedCommentId':'<:5:>',
                                'AddedBy':'<:6:>'
                            }
                     }";
        }
    }
}
