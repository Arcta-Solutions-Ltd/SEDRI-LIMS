using arc.app.Common;

namespace arc.app.Config.Queries.Export;

/// <summary>
/// Query definition for loading export schedules for a given export profile.
/// Used by the embedded schedules list on the export profile record view.
/// </summary>
internal class ExportScheduleListByProfileIdQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the export schedule list query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ExportScheduleListByProfileId',
            'TableName': 'exportschedule',
            'Type': 'Special'
        }";
    }
}
