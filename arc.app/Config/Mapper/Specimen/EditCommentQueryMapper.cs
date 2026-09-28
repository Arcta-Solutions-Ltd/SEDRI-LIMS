using arc.app.Common;

namespace arc.app.Config.Mapper.Specimen
{
    internal class EditCommentQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'editcommentquerymapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'CommentTypeId', Value: 'CommentTypeId' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Comment', Value: 'Comment' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'DisplayOnReport', Value: 'DisplayOnReport' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'CannedCommentId', Value: 'CannedCommentId' },
                        ],
                        'Target': 
                            {
                                'CommentType':'<:1:>',
                                'Comment':'<:2:>',
                                'PrintOnReport':'<:3:>',
                                'Id':'<:4:>',
                                'CannedComment':'<:5:>',
                                'CannedCommentId':'<:5:>',
                            }
                     }";
        }
    }
}
