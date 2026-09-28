using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class PatientCommentMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'patientcommentmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Comment', Value: 'Comment' }
                        ],
                        'Target': 
                            {
                                'Id': '<:1:>',
                                'PatientId':'<:1:>',
                                'Comment':'<:2:>'
                            }
                     }";
        }
    }
}
