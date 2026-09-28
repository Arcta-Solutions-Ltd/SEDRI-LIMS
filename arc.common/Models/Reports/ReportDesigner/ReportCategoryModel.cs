
using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a category that organizes report sections into logical groups.
/// Categories help structure reports by grouping related sections together.
/// </summary>
public class ReportCategoryModel
{
    /// <summary>
    /// Gets or sets the display name of the category (e.g., "Main Sections", "Organism Sections").
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the reference name that this category links to (e.g., "Main", "Organism").
    /// This links the category to a reference element in the References array.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the canonical section source name (e.g., "MainSections", "FinalSections").
    /// </summary>
    public string SourceName { get; set; }

    /// <summary>
    /// Gets or sets the data source the section source draws from as stored on the report
    /// (e.g., "Main", "Organism"). This is not always the same as <see cref="Type"/>: FinalSections
    /// is a Main-sourced category, so the stored value is round-tripped rather than derived.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the list of section names that are currently included in this category.
    /// </summary>
    public List<string> Sections { get; set; }
    public string State { get; set; }
}

/// <summary>
/// Represents a field that can be used within a report category, including
/// the internal field name and a human-friendly label.
/// </summary>
public class ReportAvailableFieldModel
{
    /// <summary>
    /// Gets or sets the internal name/identifier of the field.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the user-friendly label of the field.
    /// </summary>
    public string Label { get; set; }
}
