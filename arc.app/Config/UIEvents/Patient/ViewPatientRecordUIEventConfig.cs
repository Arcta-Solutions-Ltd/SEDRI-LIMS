using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "View Patient Record" UI event.
/// </summary>
internal class ViewPatientRecordUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "View Patient Record" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event name, description, type, and action 
    /// for viewing a patient record within the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, containing its metadata.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'viewpatientrecorduievent',
                        description: 'View patient record',
                        type: 'view-record',
                        action: 'patientrecordview'
                    }";

        return newEvent;
    }
}
