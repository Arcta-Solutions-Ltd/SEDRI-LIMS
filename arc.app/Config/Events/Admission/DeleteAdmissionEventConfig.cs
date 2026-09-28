using arc.app.Common;

namespace arc.app.Config.Events.Admission;

/// <summary>
/// Configuration for the delete admission event.
/// </summary>
internal class DeleteAdmissionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event definition for deleting an admission record.
    /// </summary>
    /// <returns>A JSON string containing the event configuration.</returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleteadmission',
                        Description: '@NeoAdmDel@',
                        EventType: 'deletedata',
                        Topic: 'Admission',
                        TableName: 'Admission',
                        RequiresSpecimenWorkflow: false,
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@GenId@' }
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'doesadmissioncontainspecimenscheckquery', message: '@NeoAdmDelB@' },
                            { type: 'NoRecord', query: 'doesadmissioncontainrequestscheckquery', message: '@NeoAdmDelC@' }
                        ]
                    }";
    }
}
