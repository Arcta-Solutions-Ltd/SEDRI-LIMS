using arc.app.Common;

namespace arc.app.Config.Events.Export;

/// <summary>
/// Event configuration for editing an export schedule.
/// </summary>
internal class EditExportScheduleEventConfig : IDefinition
{
    /// <summary>
    /// Returns the event configuration for edit export schedule.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'editexportschedule',
            Description: '@ExpSchEdit@',
            EventType: 'special',
            Topic: 'Export',
            TableName: 'ExportSchedule',
            RequiresSpecimenWorkflow: false
        }";
    }
}
