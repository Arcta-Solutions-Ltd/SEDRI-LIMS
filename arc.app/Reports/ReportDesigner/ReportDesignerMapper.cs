using arc.common;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.ReportsConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Reports.ReportDesigner;

/// <summary>
/// Mapper class to convert ReportConfig and view information to ReportDesignerModel.
/// </summary>
public class ReportConfigToReportDesignerModelMapper : IMapType<(ReportConfig reportConfig, ViewInfo viewInfo), ReportDesignerModel>
{
    /// <summary>
    /// Maps a ReportConfig and view information to a ReportDesignerModel.
    /// </summary>
    /// <param name="source">A tuple containing the report configuration and view information.</param>
    /// <returns>A ReportDesignerModel with mapped data.</returns>
    public ReportDesignerModel Map((ReportConfig reportConfig, ViewInfo viewInfo) source)
    {
        var (reportConfig, viewInfo) = source;

        var reportDesignerModel = new ReportDesignerModel
        {
            Name = reportConfig.Name,
            Title = reportConfig.Title,
            View = viewInfo?.ViewName,
            Header = reportConfig.Header,
            Footer = reportConfig.Footer,
            IncludeAlerts = reportConfig.IncludeAlerts,
            Enabled = "Yes", // Default to enabled, can be determined from view configuration if needed
            CanAddCategories = true, // Allow adding categories for now
            Categories = BuildCategoriesFromReportConfig(reportConfig), // FIX: Build categories here
            References = BuildReferencesFromViewInfo(viewInfo), // FIX: Build references here
            SectionDefinitions = new List<ReportSectionModel>(), // Will be populated when section definitions are available
            HeaderSectionDefinitions = new List<ReportSectionModel>(), // FIX: Initialize HeaderSectionDefinitions
            FooterSectionDefinitions = new List<ReportSectionModel>(), // FIX: Initialize FooterSectionDefinitions
            DataSectionsDefinition = new List<DataSectionDefinitionModel>(), // FIX: Initialize DataSectionsDefinition
            CustomFormats = new List<CustomFormatModel>(), // Will be populated when custom formats are available
            CalculatedFields = new List<ReportAvailableFieldModel>(), // Will be populated when calculated fields are available
            AllowedHeaders = viewInfo?.AllowedHeaders, // FIX: Set from viewInfo
            AllowedFooters = viewInfo?.AllowedFooters // FIX: Set from viewInfo
        };

        return reportDesignerModel;
    }


    /// <summary>
    /// Parses the comma-separated datasections string into a list.
    /// </summary>
    /// <param name="dataSectionsString">The comma-separated string of datasections.</param>
    /// <returns>A list of datasection names.</returns>
    public static List<string> ParseDataSections(string dataSectionsString)
    {
        if (string.IsNullOrEmpty(dataSectionsString))
        {
            return new List<string>();
        }

        return dataSectionsString.Split(',')
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToList();
    }

    /// <summary>
    /// Builds report categories from the ReportConfig section lists.
    /// </summary>
    /// <param name="reportConfig">The report configuration containing section lists.</param>
    /// <returns>A list of ReportCategoryModel objects.</returns>
    private static List<ReportCategoryModel> BuildCategoriesFromReportConfig(ReportConfig reportConfig)
    {
        var categories = new List<ReportCategoryModel>();

        if (reportConfig == null)
            return categories;

        // Add Main Sections category (even if currently empty) so designer can add sections
        if (reportConfig.MainSections != null)
        {
            categories.Add(new ReportCategoryModel
            {
                Name = "Main Sections",
                Type = "Main",
                SourceName = "MainSections",
                Sections = reportConfig.MainSections.ToList()
            });
        }

        // Add Organism Sections category when present
        if (reportConfig.OrganismSections != null)
        {
            categories.Add(new ReportCategoryModel
            {
                Name = "Organism Sections",
                Type = "Organism",
                SourceName = "OrganismSections",
                Sections = reportConfig.OrganismSections.ToList()
            });
        }

        // Add Final Sections category (even if empty) so designer can add sections later
        if (reportConfig.FinalSections != null)
        {
            categories.Add(new ReportCategoryModel
            {
                Name = "Final Sections",
                Type = "Final",
                SourceName = "FinalSections",
                Sections = reportConfig.FinalSections.ToList()
            });
        }

        return categories;
    }

    /// <summary>
    /// Builds report references from the ViewInfo report categories.
    /// </summary>
    /// <param name="viewInfo">The view information containing report categories.</param>
    /// <returns>A list of ReportReferenceModel objects.</returns>
    private static List<ReportReferenceModel> BuildReferencesFromViewInfo(ViewInfo viewInfo)
    {
        var references = new List<ReportReferenceModel>();

        if (viewInfo?.ReportCategories == null)
            return references;

        foreach (var viewCategory in viewInfo.ReportCategories)
        {
            references.Add(new ReportReferenceModel
            {
                Name = viewCategory.Name,
                AvailableDataSections = ParseDataSections(viewCategory.DataSections),
                AvailableFields = new List<ReportAvailableFieldModel>()
            });
        }

        return references;
    }
}
