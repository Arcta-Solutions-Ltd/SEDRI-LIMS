using System;
using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig;

/// <summary>
/// Represents a configurable section containing labeled fields and grid-based data entries.
/// </summary>
public class DataSectionConfig
{
    /// <summary>
    /// The internal identifier for the section.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The display title shown for the section.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// A list of labeled data fields associated with this section.
    /// </summary>
    public List<DataSectionFieldConfig> Fields { get; set; } = [];

    /// <summary>
    /// A list of grid-based data entries associated with this section.
    /// </summary>
    public List<DataSectionGridConfig> Grids { get; set; } = [];

    /// <summary>
    /// Updates the name of a field or grid entry by matching its current value or name.
    /// </summary>
    /// <param name="oldName">The existing name or value to search for.</param>
    /// <param name="newName">The new name or value to assign.</param>
    public void ChangeFieldName(string oldName, string newName)
    {
        if (Fields != null)
        {
            for (var fieldNum = 0; fieldNum < Fields.Count; fieldNum++)
            {
                if (Fields[fieldNum].Value.Equals(oldName, StringComparison.OrdinalIgnoreCase))
                {
                    Fields[fieldNum].Value = newName;
                }
            }
        }

        if (Grids != null)
        {
            for (var gridNum = 0; gridNum < Grids.Count; gridNum++)
            {
                if (Grids[gridNum].Name.Equals(oldName, StringComparison.OrdinalIgnoreCase))
                {
                    Grids[gridNum].Name = newName;
                    Grids[gridNum].Data = newName;
                }
            }
        }
    }

    /// <summary>
    /// Adds a new field or grid entry to the section based on the specified type.
    /// For <c>fieldgrid</c>, upserts by stable field id: if <see cref="DataSectionGridConfig.Data"/> already matches <paramref name="value"/>, updates <see cref="DataSectionGridConfig.Name"/> only when it differs.
    /// </summary>
    /// <param name="label">Display label for scalar fields (<see cref="DataSectionFieldConfig.Label"/>); for grids, becomes the designer-facing <see cref="DataSectionGridConfig.Description"/> (display only — matching uses <see cref="DataSectionGridConfig.Name"/> / <see cref="DataSectionGridConfig.Data"/> ids).</param>
    /// <param name="value">Stable field identifier (<see cref="DataSectionFieldConfig.Value"/> for scalar fields; parent fieldgrid id for grids — not translated text).</param>
    /// <param name="type">The type of entry to add (<c>fieldgrid</c> for grids, otherwise field).</param>
    public void AddField(string label, string value, string type)
    {
        if (type == "fieldgrid")
        {
            Grids ??= [];
            var existing = Grids.Find(g =>
                g.Data != null && g.Data.Equals(value, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                if (!existing.Name.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    existing.Name = value;
                }

                if (IsGridDescriptionMissing(existing.Description) && !string.IsNullOrWhiteSpace(label))
                {
                    existing.Description = label;
                }

                return;
            }

            var newGridEntry = new DataSectionGridConfig
            {
                Name = value,
                Data = value,
                Description = string.IsNullOrWhiteSpace(label) ? null : label
            };
            Grids.Add(newGridEntry);
        }
        else
        {
            var newFieldEntry = new DataSectionFieldConfig { Label = label, Value = value };
            Fields.Add(newFieldEntry);
        }
    }

    /// <summary>
    /// Removes a scalar field whose <see cref="DataSectionFieldConfig.Value"/> matches, and any grid whose <see cref="DataSectionGridConfig.Data"/> matches <paramref name="value"/> (field id, ordinal case-insensitive).
    /// </summary>
    /// <param name="value">Stable field identifier to remove.</param>
    public void DeleteField(string value)
    {
        Fields?.RemoveAll(f => f.Value.Equals(value, StringComparison.OrdinalIgnoreCase));
        Grids?.RemoveAll(g =>
            g.Data != null && g.Data.Equals(value, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Renames the data value of a grid entry that matches the specified old name.
    /// </summary>
    /// <param name="oldName">The current data value to match.</param>
    /// <param name="newName">The new data value to assign.</param>
    public void RenameGrid(string oldName, string newName)
    {
        foreach (var grid in Grids)
        {
            if (grid.Data.Equals(oldName, StringComparison.OrdinalIgnoreCase))
            {
                grid.Data = newName;
            }
        }
    }

    private static bool IsGridDescriptionMissing(string description) =>
        string.IsNullOrWhiteSpace(description)
        || description.Equals("unknown", StringComparison.OrdinalIgnoreCase);
}
