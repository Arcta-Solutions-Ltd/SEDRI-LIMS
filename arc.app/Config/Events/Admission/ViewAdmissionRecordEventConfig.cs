using arc.app.Common;

namespace arc.app.Config.Events.Admission;

/// <summary>
/// Synthetic event used for permissioning the viewadmissionrecord UI event.
/// </summary>
internal class ViewAdmissionRecordEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event definition for viewing an admission record.
    /// </summary>
    /// <returns>A JSON string containing the event configuration.</returns>
    public string Get()
    {
        return @"{
                        EventName: 'viewadmissionrecord',
                        Description: '@NeoAdmRec@',
                        EventType: 'special',
                        Topic: 'Admission',
                        TableName: 'Admission'
                    }";
    }
}
