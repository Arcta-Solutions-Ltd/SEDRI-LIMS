namespace arc.common.Models.Config;

/// <summary>
/// Canonical names for all configs created when cloning a direct or culture test.
/// </summary>
public sealed class CloneTestNames
{
    /// <summary>
    /// Form config name stored in the configs table (reserved name ending in <c>form</c>).
    /// </summary>
    public string FormConfigName { get; init; }

    /// <summary>
    /// Save event name (form config name with trailing <c>form</c> removed).
    /// </summary>
    public string SaveEventName { get; init; }

    /// <summary>
    /// Data section config name (<see cref="FormConfigName"/> + <c>datasection</c>).
    /// </summary>
    public string DataSectionName { get; init; }

    /// <summary>
    /// Report section config name (<see cref="FormConfigName"/> + <c>section</c>).
    /// </summary>
    public string ReportSectionName { get; init; }

    /// <summary>
    /// UI event config name (<see cref="FormConfigName"/> + <c>uievent</c>).
    /// </summary>
    public string UIEventName { get; init; }
}
