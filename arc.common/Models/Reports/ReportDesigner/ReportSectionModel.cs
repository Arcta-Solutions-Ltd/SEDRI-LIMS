using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a complete definition of a report section, containing all the configuration
/// and layout information needed to render the section in a report.
/// </summary>
public class ReportSectionModel
{
    /// <summary>
    /// Gets or sets the configs table identity of this section, when it was loaded from the database.
    /// Null for a section the designer has just created. Never written into configs.contents.
    /// </summary>
    public int? ConfigId { get; set; }

    /// <summary>
    /// Gets or sets the configs table type this section belongs to.
    /// </summary>
    /// <remarks>
    /// Report sections are not all one type: main and final sections are stored under one type and
    /// organism sections under another, and headers and footers have types of their own. Carrying the
    /// type with the section is what lets a save update the record it was loaded from, rather than
    /// assuming a type, failing to match, and inserting a renamed duplicate.
    /// Transient: used to drive the save and never written into configs.contents.
    /// </remarks>
    public int? ConfigTypeId { get; set; }

    /// <summary>
    /// Gets or sets which part of the report this section belongs to, as one of the
    /// <see cref="ConfigScope"/> values.
    /// </summary>
    /// <remarks>
    /// Only needed for a section the designer has just created, which has no <see cref="ConfigTypeId"/>
    /// yet. It lets the designer say where the section came from without knowing the type numbers.
    /// Transient: used to drive the save and never written into configs.contents.
    /// </remarks>
    public string Scope { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the report section.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the report section used for identification and reference.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the report section explaining its purpose and content.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the heading text that will be displayed at the top of this section.
    /// </summary>
    public string HeadingText { get; set; }

    /// <summary>
    /// Gets or sets the format identifier for how this section should be rendered (e.g., "Single column", "Table").
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// Gets or sets the type of the report section (e.g., 'layout', 'Absolute').
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the data section name that identifies the data source and structure for this section.
    /// </summary>
    public string DataSection { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this section is dynamic and can change based on data content.
    /// </summary>
    public bool Dynamic { get; set; }

    /// <summary>
    /// Gets or sets the list of fields that are displayed in this section.
    /// </summary>
    public List<ReportSectionFieldModel> Fields { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of grids that are displayed in this section.
    /// </summary>
    public List<ReportSectionGridModel> Grids { get; set; } = [];

    /// <summary>
    /// Gets or sets the row arrangement of this section's field block and grids. Areas on the same row
    /// render side by side. Empty means the legacy stacked arrangement.
    /// </summary>
    public List<ReportSectionLayoutRowModel> LayoutRows { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of lines that are displayed in this section.
    /// </summary>
    public List<ReportLineModel> Lines { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of images that are displayed in this section.
    /// </summary>
    public List<ReportImageModel> Images { get; set; } = [];

    /// <summary>
    /// Gets or sets the change the designer is requesting for this section.
    /// Transient: used to drive the save and never written into configs.contents.
    /// </summary>
    public ConfigChangeState State { get; set; }

    /// <summary>
    /// Gets or sets whether a changed header or footer should update the shared record or fork for this report only.
    /// One of the <see cref="HeaderFooterSaveScope"/> values. Transient: never written into configs.contents.
    /// </summary>
    public string SaveScope { get; set; }

    /// <summary>
    /// Gets or sets how many reports reference this header or footer by name.
    /// Transient: populated on load for save-scope prompting; never written into configs.contents.
    /// </summary>
    public int? LinkedReportCount { get; set; }
}
