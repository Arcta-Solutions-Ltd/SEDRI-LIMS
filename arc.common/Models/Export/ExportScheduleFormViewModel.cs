using Newtonsoft.Json;

namespace arc.common.Models.Export;

/// <summary>
/// View model for the export schedule add/edit form.
/// Form fields: Name, OrganismId, SpecimenTypeId, etc. + Frequency, TimeOfDay, DayOfMonth, IncrementalOnly.
/// </summary>
public class ExportScheduleFormViewModel
{
    /// <summary>
    /// Schedule ID (for edit).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Export profile ID.
    /// </summary>
    public int ExportProfileId { get; set; }

    /// <summary>
    /// Schedule name (unique per profile).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Filter: OrganismId (single or comma-separated organism list item ids).
    /// </summary>
    public string? OrganismId { get; set; }

    /// <summary>
    /// Filter: SpecimenTypeId (single or comma-separated list item ids).
    /// </summary>
    public string? SpecimenTypeId { get; set; }

    /// <summary>
    /// Filter: SpecimenStateId (single or comma-separated list item ids).
    /// </summary>
    public string? SpecimenStateId { get; set; }

    /// <summary>
    /// Filter: TagId (single or comma-separated list item ids).
    /// </summary>
    public string? TagId { get; set; }

    /// <summary>
    /// Filter: OrganisationId (single or comma-separated organisation ids).
    /// </summary>
    public string? OrganisationId { get; set; }

    /// <summary>
    /// Filter: LocationId (single or comma-separated location ids).
    /// </summary>
    public string? LocationId { get; set; }

    /// <summary>
    /// Filter: TestId (single or comma-separated direct test config ids).
    /// </summary>
    public string? TestId { get; set; }

    /// <summary>
    /// Filter: AST exclusive (Yes/No).
    /// </summary>
    public string? ASTExclusive { get; set; }

    /// <summary>
    /// Frequency: hourly, daily, monthly.
    /// </summary>
    public string Frequency { get; set; } = string.Empty;

    /// <summary>
    /// Time of day (e.g. "09:00") for daily/monthly.
    /// </summary>
    public string? TimeOfDay { get; set; }

    /// <summary>
    /// Day of month (1-31) for monthly.
    /// </summary>
    public int? DayOfMonth { get; set; }

    /// <summary>
    /// Incremental only: Yes/No. Deprecated; use ChangesToInclude.
    /// </summary>
    public string? IncrementalOnly { get; set; }

    /// <summary>
    /// Enabled: Yes/No.
    /// </summary>
    public string? Enabled { get; set; }

    /// <summary>
    /// Output directory for saving export files (relative to storage root).
    /// </summary>
    public string? OutputDirectory { get; set; }

    /// <summary>
    /// Changes to include: "newonly" or "newandmodified".
    /// </summary>
    public string? ChangesToInclude { get; set; }
}
