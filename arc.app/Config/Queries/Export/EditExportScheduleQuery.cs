using arc.app.Common;

namespace arc.app.Config.Queries.Export;

/// <summary>
/// Query config for loading an export schedule for edit.
/// Type: Special - executed by SpecialFactory which returns schedule with filter expanded.
/// </summary>
internal class EditExportScheduleQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the edit export schedule query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'EditExportSchedule',
            'Type': 'Special',
            'TableName': 'exportschedule'
        }";
    }
}
