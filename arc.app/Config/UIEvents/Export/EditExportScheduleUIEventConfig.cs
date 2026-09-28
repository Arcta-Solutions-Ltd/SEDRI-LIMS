using arc.app.Common;

namespace arc.app.Config.UIEvents.Export;

/// <summary>
/// Configuration for the "Edit Export Schedule" UI event.
/// Opens the form to edit an existing export schedule.
/// </summary>
internal class EditExportScheduleUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Export Schedule" UI event.
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editexportscheduleuievent',
                        description: '@GenEdi@',
                        type: 'form',
                        action: 'editexportscheduleform'
                    }";

        return newEvent;
    }
}
