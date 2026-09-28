using arc.app.Common;

namespace arc.app.Config.Events.Request;

/// <summary>
/// Synthetic event used for permissioning the viewrequestrecord UI event.
/// </summary>
internal class ViewRequestRecordEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event definition for viewing a request record.
    /// </summary>
    /// <returns>A JSON string containing the event configuration.</returns>
    public string Get()
    {
        return @"{
                        EventName: 'viewrequestrecord',
                        Description: '@NeoReqRec@',
                        EventType: 'special',
                        Topic: 'Request',
                        TableName: 'Request'
                    }";
    }
}
