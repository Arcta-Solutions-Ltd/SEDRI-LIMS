using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class PatientDiaryResultMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'patientdiaryresultmapper', 'Type': 'Special'
                     }";
        }
    }
}
