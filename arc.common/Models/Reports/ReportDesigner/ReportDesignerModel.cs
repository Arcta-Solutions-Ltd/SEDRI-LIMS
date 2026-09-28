using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents the complete model for the report designer, containing all configuration and metadata
/// needed to design and configure reports within the system.
/// </summary>
public class ReportDesignerModel
{
    /// <summary>
    /// Gets or sets the unique name identifier for the report.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the display title of the report that will be shown to users.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the name of the view that this report belongs to (e.g., "specimens", "cultures").
    /// </summary>
    public string View { get; set; }

    /// <summary>
    /// Gets or sets the header text that will be displayed at the top of the report.
    /// </summary>
    public string Header { get; set; }

    /// <summary>
    /// Gets or sets the footer text that will be displayed at the bottom of the report.
    /// </summary>
    public string Footer { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether alerts should be included in the report output.
    /// </summary>
    public bool IncludeAlerts { get; set; }

    /// <summary>
    /// Gets or sets the enabled status of the report (e.g., "Yes", "No").
    /// </summary>
    public string Enabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether new categories can be added to this report.
    /// </summary>
    public bool CanAddCategories { get; set; }

    /// <summary>
    /// Gets or sets the list of report categories that organize the report sections.
    /// Categories typically include "Main Sections", "Organism Sections", and "Final Sections".
    /// </summary>
    public List<ReportCategoryModel> Categories { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of reference elements that contain available data sections and fields.
    /// Categories reference these by name to share the same data sources.
    /// </summary>
    public List<ReportReferenceModel> References { get; set; } = [];

    /// <summary>
    /// Gets or sets the complete definitions for all sections that are included in the report categories.
    /// This provides the detailed configuration for each section used in the report.
    /// </summary>
    public List<ReportSectionModel> SectionDefinitions { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of section definitions for all headers listed in AllowedHeaders.
    /// </summary>
    public List<ReportSectionModel> HeaderSectionDefinitions { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of section definitions for all footers listed in AllowedFooters.
    /// </summary>
    public List<ReportSectionModel> FooterSectionDefinitions { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of data section definitions for all data sections specified in the report categories.
    /// </summary>
    public List<DataSectionDefinitionModel> DataSectionsDefinition { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of custom formatting options available for the report.
    /// </summary>
    public List<CustomFormatModel> CustomFormats { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of calculated fields available for use in the report (e.g., Printed Date, Pages).
    /// </summary>
    public List<ReportAvailableFieldModel> CalculatedFields { get; set; } = [];

    /// <summary>
    /// Gets or sets the comma-separated list of header names that can be included on reports in this view.
    /// </summary>
    public string AllowedHeaders { get; set; }

    /// <summary>
    /// Gets or sets the comma-separated list of footer names that can be included on reports in this view.
    /// </summary>
    public string AllowedFooters { get; set; }
}
