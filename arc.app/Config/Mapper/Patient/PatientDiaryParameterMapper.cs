using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class PatientDiaryParameterMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'patientdiaryparametermapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Key', Where: 'Id', Value: 'Value' }
                        ],
                        'Target' : { 
                            'Name': 'PatientDiaryEntryQuery', 
                            'Parameters': [ 
                                {'Key': 'PatientId', 'Value': '<:1:>'},
                                {'Key': 'EventStatusId', 'Value': '666'}
                            ] 
                        }
                     }";
        }
    }
}
