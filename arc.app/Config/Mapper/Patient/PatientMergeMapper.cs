using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class PatientMergeMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'patientsearchparametermapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'PatientRef', Value: 'PatientRef' },
                        ],
                        'Target' :                             
                        {
                            'NewPatientRef':'<:1:>'
                        }
                     }";
        }
    }
}
