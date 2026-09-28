using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class MergePatientEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'mergepatient', 
                        Description: '@PatMerA@',
                        EventType : 'special', 
                        Topic : 'Patient', 
                        TableName: 'Patient',
                        RequiresSpecimenWorkflow: false,
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@PatAA@' }
                        ]
                    }";
        }
    }
}
