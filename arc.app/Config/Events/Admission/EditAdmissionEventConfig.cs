using arc.app.Common;

namespace arc.app.Config.Events.Admission;

/// <summary>
/// Configuration for the edit admission event.
/// </summary>
internal class EditAdmissionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event definition for editing an admission record.
    /// </summary>
    /// <returns>A JSON string containing the event configuration.</returns>
    public string Get()
    {
        return @"{
                        EventName: 'editadmission',
                        Description: '@NeoAdmEdi@',
                        EventType: 'editdata',
                        Topic: 'Admission',
                        TableName: 'Admission',
                        RequiresSpecimenWorkflow: false,
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@GenId@' }
                        ]
                    }";
    }
}
