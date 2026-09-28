using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeletePatientEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deletepatient', 
                        Description: '@PatDelA@',
                        EventType : 'deletedata', 
                        Topic : 'Patient', 
                        TableName: 'Patient',
                        RequiresSpecimenWorkflow: false,
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@PatAA@' }
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'doespatientcontainspecimenscheckquery', message: '@PatPatK@' }
                        ]
                    }";
        }
    }
}
