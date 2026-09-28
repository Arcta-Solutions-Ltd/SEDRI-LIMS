using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditPatientEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editPatient', 
                        Description: '@PatEdi@',
                        EventType : 'editdata', 
                        Topic : 'Patient', 
                        TableName: 'Patient',
                        RequiresSpecimenWorkflow: false,
                        Mapping: 'editpatientmapper',
                        DataRules: [
                            {
                              'type': 'NoRecord',
                              'query': 'patientrefexists',
                              'message': '@PatRefA@'
                            }
                          ],
                        ValidationRules: [
                            { field: 'PatientRef', rule: 'required', message: '@PatPat@'},
                            { field: 'Surname', rule: 'required', message: '@PatSur@'},
                            { field: 'Gender', rule: 'required', message: '@PatGen@'}
                        ]
                    }";
        }

    }
}
