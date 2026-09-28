using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class LaboratorySpecimenCountMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'laboratoryspecimencountmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'laboratoryspecimencount', 
                            'Parameters': [ 
                                {'Key': 'LaboratoryId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
