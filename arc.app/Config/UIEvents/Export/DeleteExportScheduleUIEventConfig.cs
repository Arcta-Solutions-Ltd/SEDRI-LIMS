using arc.app.Common;

namespace arc.app.Config.UIEvents.Export;

/// <summary>
/// Configuration for the "Delete Export Schedule" UI event.
/// Opens the form to delete an export schedule.
/// </summary>
internal class DeleteExportScheduleUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Export Schedule" UI event.
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deleteexportscheduleuievent',
                        description: '@GenDel@',
                        type: 'form',
                        action: 'deleteexportscheduleform'
                    }";

        return newEvent;
    }
}
