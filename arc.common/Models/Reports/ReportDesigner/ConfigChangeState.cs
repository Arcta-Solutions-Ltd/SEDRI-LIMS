namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Describes what the designer wants done with a configuration record when the change set is saved.
/// Replaces the previous loose combination of a string State and a bool IsNew flag.
/// </summary>
public enum ConfigChangeState
{
    /// <summary>
    /// The record is unchanged. Treated as <see cref="Modified"/> when it still arrives in a change list.
    /// </summary>
    Unchanged = 0,

    /// <summary>
    /// The record does not exist in the configs table yet and must be inserted.
    /// </summary>
    Added = 1,

    /// <summary>
    /// The record already exists in the configs table and must be updated.
    /// </summary>
    Modified = 2,

    /// <summary>
    /// The record must be removed from the configs table.
    /// </summary>
    Deleted = 3
}
