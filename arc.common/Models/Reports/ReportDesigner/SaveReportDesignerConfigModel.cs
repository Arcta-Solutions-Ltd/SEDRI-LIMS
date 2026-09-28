using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Model for receiving report designer configuration changes from the frontend.
/// Contains the complete report definition along with any changed section definitions and custom formats.
/// </summary>
public class SaveReportDesignerConfigModel
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
    public List<ReportCategoryModel> Categories { get; set; } = new List<ReportCategoryModel>();

    /// <summary>
    /// Gets or sets the list of section definitions that have been changed and need to be saved.
    /// These represent new or modified section configurations.
    /// </summary>
    public List<ReportSectionModel> ChangedSectionDefinitions { get; set; } = new List<ReportSectionModel>();

    /// <summary>
    /// Gets or sets the list of custom formats that have been changed and need to be saved.
    /// These represent new or modified formatting configurations.
    /// </summary>
    public List<CustomFormatModel> ChangedCustomFormats { get; set; } = new List<CustomFormatModel>();

    /// <summary>
    /// Gets or sets the section sources for the report. When empty the backend falls back to the
    /// fixed Main/Organism/Final set, which preserves the behaviour of reports saved before
    /// category changes were persisted.
    /// </summary>
    public List<SectionSourceModel> SectionSources { get; set; } = new List<SectionSourceModel>();

    /// <summary>
    /// Gets or sets the sections the designer has removed. Their configs records are deleted as part
    /// of the same transaction as the rest of the save.
    /// </summary>
    public List<ConfigReferenceModel> DeletedSections { get; set; } = new List<ConfigReferenceModel>();

    /// <summary>
    /// Gets or sets the custom formats the designer has removed. Their configs records are deleted as part
    /// of the same transaction as the rest of the save.
    /// </summary>
    public List<ConfigReferenceModel> DeletedFormats { get; set; } = new List<ConfigReferenceModel>();
}
