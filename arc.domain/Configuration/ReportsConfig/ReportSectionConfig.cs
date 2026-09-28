using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.ReportsConfig;

/// <summary>
/// Represents the configuration for a report section.
/// </summary>
public class ReportSectionConfig
{
    /// <summary>
    /// Gets or sets the identifier of the report section.
    /// </summary>
    /// <required>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the report section, used to identify and locate the configuration.
    /// </summary>
    /// <required>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the report section.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the heading text of the report section.
    /// </summary>
    public string HeadingText { get; set; }

    /// <summary>
    /// Gets or sets the format of the report section.
    /// </summary>
    public string Format { get; set; }

    /// <summary>
    /// Gets or sets the type of the report section (e.g., 'layout', 'Absolute').
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the data section of the report section.
    /// Necessary to find and load the correct data for the section.
    /// </summary>
    /// <required>
    public string DataSection { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the report section is dynamic.
    /// </summary>
    public bool Dynamic { get; set; }

    /// <summary>
    /// Gets or sets the list of fields in the report section.
    /// </summary>
    public List<ReportSectionFieldConfig> Fields { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of grids in the report section.
    /// </summary>
    public List<ReportSectionGridConfig> Grids { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of lines in the report section.
    /// </summary>
    public List<LineConfig> Lines { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of images in the report section.
    /// </summary>
    public List<ImageConfig> Images { get; set; } = [];

    /// <summary>
    /// Gets or sets the row arrangement of this section's field block and grids. Areas on the same row
    /// render side by side.
    /// </summary>
    /// <remarks>
    /// Empty or null means the legacy stacked arrangement: the field block first, then each grid beneath it.
    /// Sections saved before area rows existed therefore keep rendering exactly as they did.
    /// </remarks>
    public List<ReportSectionLayoutRowConfig> LayoutRows { get; set; } = [];

    /// <summary>
    /// The number of pixels to have between lines
    /// </summary>
    public int LineSpacing { get; set; } = 2;

    /// <summary>
    /// Gets or sets the separator drawn after the section, which makes the renderer add a horizontal rule
    /// below the section's content.
    /// </summary>
    public string Separator { get; set; }

    /// <summary>
    /// The format a section falls back to when it gains a grid and its current format has no grid
    /// positions at all. A format that already defines grid positions is left alone.
    /// </summary>
    public const string DefaultGridFormat = "DoubleColumnWithGridOne";

    /// <summary>
    /// Adds a field to the report section.
    /// For <c>fieldgrid</c>, <paramref name="value"/> is the stable parent field id stored as <see cref="ReportSectionGridConfig.Name"/>; duplicate ids are ignored (idempotent).
    /// </summary>
    /// <param name="label">The label of the field.</param>
    /// <param name="value">Field identifier for scalar fields (<see cref="ReportSectionFieldConfig.Value"/>); for <c>fieldgrid</c>, the parent field id on <see cref="ReportSectionGridConfig.Name"/>.</param>
    /// <param name="type">The type of the field.</param>
    /// <param name="column">The column of the field.</param>
    /// <param name="order">The order of the field.</param>
    /// <param name="gridPositionsInCurrentFormat">
    /// How many grid positions <see cref="Format"/> defines. Supplied by the caller because the format
    /// catalogue is not visible from here. When it is greater than zero the format is kept, so a section
    /// configured with a format that places several grids is not reset to the single grid default.
    /// </param>
    public void AddField(string label, string value, string type, int column = 0, int order = 0, int gridPositionsInCurrentFormat = 0)
    {
        if (type == "fieldgrid")
        {
            Grids ??= [];
            if (Grids.Any(g => g.Name != null && g.Name.Equals(value, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            var newGrid = new ReportSectionGridConfig { Name = value };
            Grids.Add(newGrid);

            if (gridPositionsInCurrentFormat <= 0 && Format != DefaultGridFormat && Format != "DoubleColumnWithGridTwo")
            {
                Format = DefaultGridFormat;
            }

            return;
        }

        column = column == 0 ? 1 : column;
        order = order == 0 ? Fields.Count == 0 ? 1 : Fields.Max(f => f.Order) + 1 : order;
        var newField = new ReportSectionFieldConfig { Label = label, Value = value, Column = column, Order = order };
        Fields.Add(newField);
    }

    /// <summary>
    /// Clears all fields from the report section.
    /// </summary>
    public void ClearFields()
    {
        Fields.Clear();
    }

    /// <summary>
    /// Changes the name of a field in the report section.
    /// </summary>
    /// <param name="oldName">The old name of the field.</param>
    /// <param name="newName">The new name of the field.</param>
    public void ChangeFieldName(string oldName, string newName)
    {
        if (Fields != null)
        {
            for (var fieldNum = 0; fieldNum < Fields.Count; fieldNum++)
            {
                if (Fields[fieldNum].Value.Equals(oldName, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    Fields[fieldNum].Value = newName;
                }
            }
        };

        if (Grids != null)
        {
            for (var gridNum = 0; gridNum < Grids.Count; gridNum++)
            {
                if (Grids[gridNum].Name.Equals(oldName, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    Grids[gridNum].Name = newName;
                }
            }
        }
    }

    /// <summary>
    /// Deletes a scalar field whose <see cref="ReportSectionFieldConfig.Value"/> matches, and any grid whose <see cref="ReportSectionGridConfig.Name"/> matches <paramref name="value"/> (parent fieldgrid field id).
    /// </summary>
    /// <param name="value">Stable field identifier to remove.</param>
    public void DeleteField(string value)
    {
        if (Fields.Count > 0)
        {
            var fieldToDelete = Fields.FirstOrDefault(f => f.Value.Equals(value, StringComparison.OrdinalIgnoreCase));

            if (fieldToDelete != null)
            {
                Fields.Remove(fieldToDelete);
            }
        }

        Grids?.RemoveAll(g => g.Name != null && g.Name.Equals(value, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Resets the columns of the fields in the report section.
    /// </summary>
    /// <param name="columnNumber">The number of columns to reset to.</param>
    public void ResetColumns(int columnNumber)
    {
        if (Fields != null)
        {
            var number = 1;
            for (var fieldNum = 0; fieldNum < Fields.Count; fieldNum++)
            {
                Fields[fieldNum].Column = number;
                number = number == columnNumber ? 1 : number + 1;
            }
        };
    }

    /// <summary>
    /// Renames a grid in the report section.
    /// </summary>
    /// <param name="oldName">The old name of the grid.</param>
    /// <param name="newName">The new name of the grid.</param>
    public void RenameGrid(string oldName, string newName)
    {
        foreach (var grid in Grids)
        {
            if (grid.Name.Equals(oldName, System.StringComparison.CurrentCultureIgnoreCase))
            {
                grid.Name = newName;
            }
        }
    }
}
