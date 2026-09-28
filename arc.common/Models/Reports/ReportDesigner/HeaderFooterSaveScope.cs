namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Save intent for a header or footer definition when more than one report references it.
/// </summary>
/// <remarks>
/// Transient designer state: never written into configs.contents.
/// </remarks>
public static class HeaderFooterSaveScope
{
    /// <summary>
    /// Update the shared header or footer record used by every report that references it.
    /// </summary>
    public const string Shared = "Shared";

    /// <summary>
    /// Fork the header or footer for the current report only, leaving other reports on the original record.
    /// </summary>
    public const string CurrentReportOnly = "CurrentReportOnly";
}
