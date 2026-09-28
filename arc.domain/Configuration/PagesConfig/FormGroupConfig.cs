using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.PagesConfig;

/// <summary>
/// Represents a group of fields within a form page column. Fields within a group are visually grouped together.
/// </summary>
public class FormGroupConfig
{
    /// <summary>
    /// Gets or sets the unique identifier key for this form group.
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Gets or sets the column index this form group belongs to.
    /// </summary>
    public int Column { get; set; }
    /// <summary>
    /// Gets or sets the separator text displayed between form groups.
    /// </summary>
    public string Separator { get; set; }
    /// <summary>
    /// Gets or sets the list of field configurations contained in this form group.
    /// </summary>
    public List<FieldConfig> Fields { get; set; } = new List<FieldConfig>();
    /// <summary>
    /// Gets or sets the list of validation and visibility rules that apply to this form group.
    /// </summary>
    public List<RuleConfig> Rules { get; set; }

    /// <summary>
    /// Adds a field to this form group at the specified position or at the end.
    /// </summary>
    /// <param name="newField">The field configuration to add.</param>
    /// <param name="orderIndex">The position to insert the field at. If -1, adds to the end.</param>
    internal void AddField(FieldConfig newField, int orderIndex = -1)
    {
        if (orderIndex < 0)
        {
            Fields.Add(newField);
        }
        else
        {
            Fields.Insert(orderIndex, newField);
        }
    }

    /// <summary>
    /// Removes a field from this form group by name (case-insensitive match).
    /// </summary>
    /// <param name="fieldName">The identifier of the field to remove.</param>
    internal void DeleteField(string fieldName)
    {
        var fields = new List<FieldConfig>();
        foreach (var field in Fields)
        {
            if (!field.Id.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase))
            {
                fields.Add(field);
            }
        }
        Fields = fields;
    }

    /// <summary>
    /// Removes all fields from this form group.
    /// </summary>
    internal void DeleteAllFields()
    {
        Fields = new List<FieldConfig>();
    }

    /// <summary>
    /// Returns all grid subfield configurations from fields in this group, optionally filtered by parent field name.
    /// </summary>
    /// <param name="fieldName">Optional parent field identifier filter. If empty or null, returns grid fields from all fields in this group.</param>
    /// <returns>A list of grid field configurations.</returns>
    internal List<FieldGridConfig> GetGridFieldList(string fieldName)
    {
        var returnList = new List<FieldGridConfig>();
        foreach (var field in Fields)
        {
            if (string.IsNullOrWhiteSpace(fieldName) || field.Id.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase))
            {
                returnList.AddRange(field.GridFields);
            }
        };
        return returnList;
    }

    internal IEnumerable<string> GetUIEvents()
    {
        var formEvents = Fields.Where(f => !string.IsNullOrEmpty(f.FormUIEvent)).Select(f => f.FormUIEvent);
        var addFormEvents = Fields.Where(f => !string.IsNullOrEmpty(f.AddFormUIEvent)).Select(f => f.AddFormUIEvent);

        var result = formEvents.Concat(addFormEvents);
            
        return result;
    }
}
