using arc.app.Common;

namespace arc.app.Config.Events.Request;

/// <summary>
/// Configuration for the delete request event.
/// </summary>
internal class DeleteRequestEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event definition for deleting a request record.
    /// </summary>
    /// <returns>A JSON string containing the event configuration.</returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleterequest',
                        Description: '@NeoReqDel@',
                        EventType: 'deletedata',
                        Topic: 'Request',
                        TableName: 'Request',
                        RequiresSpecimenWorkflow: false,
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@GenId@' }
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'doesrequestcontainspecimenscheckquery', message: '@NeoReqDelB@' }
                        ]
                    }";
    }
}
