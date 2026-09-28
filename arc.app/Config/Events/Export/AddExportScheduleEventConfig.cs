using arc.app.Common;

namespace arc.app.Config.Events.Export;

/// <summary>
/// Event configuration for adding an export schedule.
/// </summary>
internal class AddExportScheduleEventConfig : IDefinition
{
    /// <summary>
    /// Returns the event configuration for add export schedule.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'addexportschedule',
            Description: '@ExpSchAdd@',
            EventType: 'special',
            Topic: 'Export',
            TableName: 'ExportSchedule',
            RequiresSpecimenWorkflow: false,
            ValidationRules: [
                { field: 'Name', rule: 'required', message: '@GenNamReq@' }
            ],
            DataRules: [
                { type: 'NoRecord', query: 'exportschedulealreadyexistsforadd', message: '@ExpSchDup@' }
            ]
        }";
    }
}
