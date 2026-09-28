using arc.app.Common;

namespace arc.app.Config.Events.Request;

/// <summary>
/// Configuration for the edit request event.
/// </summary>
internal class EditRequestEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event definition for editing a request record.
    /// </summary>
    /// <returns>A JSON string containing the event configuration.</returns>
    public string Get()
    {
        return @"{
                        EventName: 'editrequest',
                        Description: '@NeoReqEdi@',
                        EventType: 'editdata',
                        Topic: 'Request',
                        TableName: 'Request',
                        RequiresSpecimenWorkflow: false,
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@GenId@' }
                        ]
                    }";
    }
}
