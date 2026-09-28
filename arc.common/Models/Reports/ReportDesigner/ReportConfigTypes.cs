namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// The configs table types the report designer reads and writes.
/// </summary>
/// <remarks>
/// These are shared rather than declared alongside each usage because the loading side in arc.app and
/// the saving side in arc.data have to agree on them. When they did not, a section loaded from one
/// type was saved to another, matched nothing, and was inserted as a renamed duplicate.
/// </remarks>
public static class ReportConfigTypes
{
    /// <summary>
    /// Report records.
    /// </summary>
    public const int Report = 13;

    /// <summary>
    /// Sections belonging to the main and final categories of a report.
    /// </summary>
    public const int MainSection = 14;

    /// <summary>
    /// Sections belonging to the organism category of a report.
    /// </summary>
    public const int OrganismSection = 15;

    /// <summary>
    /// Report section formats. Added by v3.00.04/004-report-section-format-configtype.sql, which split
    /// them away from the settings type they previously shared with the reportstatus record.
    /// </summary>
    public const int SectionFormat = 23;

    /// <summary>
    /// The type section formats were stored under before the split. Still read as a fallback for
    /// databases that have not run the migration.
    /// </summary>
    public const int LegacySectionFormat = 21;

    /// <summary>
    /// Report headers. Added by v3.00.04/005-report-header-footer-configtype.sql.
    /// </summary>
    public const int ReportHeader = 24;

    /// <summary>
    /// Report footers. Added by v3.00.04/005-report-header-footer-configtype.sql.
    /// </summary>
    public const int ReportFooter = 25;
}
