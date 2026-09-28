using arc.app.Common;

namespace arc.app.Config.UIEvents.Export;

/// <summary>
/// Configuration for the "Add Export Schedule" UI event.
/// Opens the form to add a new schedule for an export profile.
/// </summary>
internal class AddExportScheduleUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Export Schedule" UI event.
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addexportscheduleuievent',
                        description: '@ExpSchAdd@',
                        type: 'form',
                        action: 'addexportscheduleform',
                        recordView: 'exportprofilerecordview'
                    }";

        return newEvent;
    }
}
