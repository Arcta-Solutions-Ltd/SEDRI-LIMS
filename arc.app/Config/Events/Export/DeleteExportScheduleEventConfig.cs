using arc.app.Common;

namespace arc.app.Config.Events.Export;

/// <summary>
/// Event configuration for deleting an export schedule.
/// </summary>
internal class DeleteExportScheduleEventConfig : IDefinition
{
    /// <summary>
    /// Returns the event configuration for delete export schedule.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'deleteexportschedule',
            Description: '@ExpSchDel@',
            EventType: 'special',
            Topic: 'Export',
            TableName: 'ExportSchedule',
            RequiresSpecimenWorkflow: false
        }";
    }
}
