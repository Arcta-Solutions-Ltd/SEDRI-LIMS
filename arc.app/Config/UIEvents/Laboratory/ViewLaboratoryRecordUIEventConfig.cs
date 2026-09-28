using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "View Laboratory Record" UI event.
/// </summary>
internal class ViewLaboratoryRecordUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "View Laboratory Record" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event name, description, type, and action 
    /// associated with viewing a laboratory record.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, containing relevant metadata.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'viewlaboratoryrecorduievent',
                        description: 'View laboratory record',
                        type: 'view-record',
                        action: 'laboratories'
                    }";

        return newEvent;
    }
}
