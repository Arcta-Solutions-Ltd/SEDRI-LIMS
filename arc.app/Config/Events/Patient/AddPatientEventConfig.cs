using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddPatientEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addPatient', 
                        Description: '@PatAdd@',
                        EventType : 'adddata', 
                        Topic : 'Patient', 
                        TableName: 'Patient',
                        RequiresSpecimenWorkflow: false,
                        Mapping: 'addpatientmapper',
                        ValidationRules: [
                            { field: 'PatientRef', rule: 'required', message: '@PatPat@' },
                            { field: 'Surname', rule: 'required', message: '@PatSur@' },
                            { field: 'Gender', rule: 'required', message: '@PatGen@' }
                        ]
                    }";
        }
    }
}
