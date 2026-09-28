using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig.ReportToPrint;

/// <summary>
/// Represents the contents of a report.
/// </summary>
public class ContentsConfig
{
    /// <summary>
    /// The name of the contents, used to identify and locate the configuration.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The column headings of a grid table, or null when the grid has no header row.
    /// </summary>
    public List<string> Head { get; set; }

    /// <summary>
    /// The renderer to dispatch to, taken from the section format's Type, except grids which are always "Table".
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The key a grid table reads its rows from in the report data's Tables collection.
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// The left margin of the contents  in pixels.
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// When set, the renderer draws a horizontal rule after this entry.
    /// </summary>
    public string Separator { get; set; }

    /// <summary>
    /// A grid table's pipe delimited column widths in points, such as "150|150|80".
    /// </summary>
    public string Width { get; set; }
    public string Colour { get; set; }

    /// <summary>
    /// The section heading line, merging the section's heading text with the format's heading geometry.
    /// Absent when the section has no heading text.
    /// </summary>
    public List<LineConfig> Heading { get; set; }

    /// <summary>
    /// The first field column of a field block, with its geometry and the fields placed in it.
    /// </summary>
    public ColumnConfig Column1 { get; set; }

    /// <summary>
    /// The second field column of a field block, or null for a single column format.
    /// </summary>
    public ColumnConfig Column2 { get; set; }

    /// <summary>
    /// The absolutely positioned lines of an absolute section.
    /// </summary>
    public List<LineConfig> Lines { get; set; } = [];
    public int LineSpacing { get; set; } = 2;

    /// <summary>
    /// The organism group this entry belongs to when the section is repeated per organism.
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// How many grid entries follow this field block and belong to the same section, which is what lets the
    /// renderer defer the section heading to the first grid and estimate the section's height for page breaks.
    /// </summary>
    public int LinkedSections { get; set; }

    /// <summary>
    /// Set to "grid" for a layout section grid, which routes it to the multi table renderer rather than the
    /// legacy single table shortcut.
    /// </summary>
    public string Theme { get; set; }

    /// <summary>
    /// Whether this entry is repeated once per item in its group, as organism sections are.
    /// </summary>
    public bool Multiple { get; set; }

    /// <summary>
    /// The data collection a repeated entry iterates over.
    /// </summary>
    public string MultipleName { get; set; }

    /// <summary>
    /// Whether fields with no data are removed and remaining fields reflowed at print time.
    /// </summary>
    public bool Dynamic { get; set; }

    /// <summary>
    /// Groups the entries that share one layout section row so the renderer draws them side by side.
    /// </summary>
    /// <remarks>
    /// Formatted as "{sectionName}#{rowIndex}". Null or empty means this entry renders on its own, advancing
    /// the cursor in document order, which is how every section behaved before area rows existed. The section
    /// name is part of the key so two sections cannot be merged into one row by sharing a row number.
    /// </remarks>
    public string LayoutRowKey { get; set; }

    /// <summary>
    /// How many areas share this entry's row. One or zero means the entry has the row to itself.
    /// </summary>
    public int LayoutRowAreaCount { get; set; }

    /// <summary>
    /// The images to be displayed in the contents.
    /// </summary>
    public List<ImageConfig> Images { get; set; } = [];

    /// <summary>
    /// When true, grid table cells render without borders in the PDF.
    /// </summary>
    public bool NoBox { get; set; }
}
