using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class PatientLabelAvailableFieldsQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'PatientLabelAvailableFields',
                        'TableName': 'Patient',
                        'Type': 'Special',
                        'ResultMapping': 'patientlabelavailablefieldsmapper'
                    }";
        }
    }
}
