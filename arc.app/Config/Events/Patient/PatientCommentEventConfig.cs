using arc.app.Common;

namespace arc.app.Config.Events
{
    public class PatientCommentEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'patientcomment', 
                        Description: '@PatEnt@',
                        EventType : 'adddata', 
                        Topic : 'Patient', 
                        TableName: 'PatientComment',
                        RequiresSpecimenWorkflow: false,
                        Mapping: 'patientcommentmapper',
                        ValidationRules: [
                            { field: 'Comment', rule: 'required', message: '@PatA@'}
                        ]
                    }";
        }
    }
}
