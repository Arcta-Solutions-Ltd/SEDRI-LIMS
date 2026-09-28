using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a custom formatting option that can be applied to report elements.
/// Contains the configuration for section formats available in the report designer.
/// </summary>
public class CustomFormatModel
{
    /// <summary>
    /// Gets or sets the configs table identity of this format, when it was loaded from the database.
    /// Null for a format the designer has just created. Never written into configs.contents.
    /// </summary>
    public int? ConfigId { get; set; }

    /// <summary>
    /// Gets or sets the name of the custom format.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the custom format.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the description of the custom format.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the heading configuration for the format.
    /// </summary>
    public List<ReportLineModel> Heading { get; set; } = [];

    /// <summary>
    /// Gets or sets the column configuration for the format.
    /// </summary>
    public List<ReportColumnModel> Columns { get; set; } = [];

    /// <summary>
    /// Gets or sets the grid configuration for the format.
    /// </summary>
    public List<ReportGridModel> Grids { get; set; } = [];

    /// <summary>
    /// Gets or sets the image configuration for the format.
    /// </summary>
    public List<ReportImageModel> Images { get; set; } = [];

    /// <summary>
    /// Gets or sets the change the designer is requesting for this format.
    /// Transient: used to drive the save and never written into configs.contents.
    /// </summary>
    public ConfigChangeState State { get; set; }
}
