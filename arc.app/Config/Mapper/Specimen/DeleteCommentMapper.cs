using arc.app.Common;

namespace arc.app.Config.Mapper.Specimen
{
    internal class DeleteCommentMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'deletecommentmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Comment', Value: 'Comment' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Value', Value: 'Value' }
                        ],
                        'Target': 
                            {
                                'Comment':'<:1:>',
                                'CannedComment':'<:2:>'
                            }
                     }";
        }
    }
}
