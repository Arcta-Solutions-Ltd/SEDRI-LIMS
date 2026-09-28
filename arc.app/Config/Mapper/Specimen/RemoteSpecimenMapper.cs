using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class RemoteSpecimenMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'remotespecimenmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            {Type: 'Mapping', Source: 'PatientLocation', Target: 'PatientLocationId'},
                            {Type: 'Mapping', Source: 'Diagnosis', Target: 'DiagnosisId'}, 
                            {Type: 'Mapping', Source: 'SpecimenType', Target: 'SpecimenTypeId'}, 
                            {Type: 'Mapping', Source: 'SpecimenSite', Target: 'SpecimenSiteId'},
                            {Type: 'Mapping', Source: 'Ward', Target: 'WardId'}
                        ]
                     }";
        }
    }
}
