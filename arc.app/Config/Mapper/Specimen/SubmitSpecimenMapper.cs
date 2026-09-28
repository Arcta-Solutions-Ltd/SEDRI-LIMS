using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class SubmitSpecimenMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'submitspecimenmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'id', Value: 'id' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>'
                            }
                     }";
        }
    }
}
