using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.PagesConfig;

/// <summary>
/// Represents a column layout within a form page. Columns contain form groups and define width constraints.
/// </summary>
public class ColumnConfig
{
    /// <summary>
    /// Gets or sets the unique identifier key for this column.
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Gets or sets the width setting for fields within this column.
    /// </summary>
    public string FieldWidth { get; set; }
    /// <summary>
    /// Gets or sets the width setting for labels within this column.
    /// </summary>
    public string LabelWidth { get; set; }
    /// <summary>
    /// Gets or sets the width setting for items within this column.
    /// </summary>
    public string ItemWidth { get; set; }
    /// <summary>
    /// Gets or sets the list of form groups contained in this column.
    /// </summary>
    public List<FormGroupConfig> FormGroups { get; set; } = new List<FormGroupConfig>();
    /// <summary>
    /// Gets or sets the list of validation and visibility rules that apply to this column.
    /// </summary>
    public List<RuleConfig> Rules { get; set; }

    /// <summary>
    /// Returns the last form group in this column.
    /// </summary>
    /// <returns>The last <see cref="FormGroupConfig"/> in the FormGroups list.</returns>
    internal FormGroupConfig GetLastFormGroup()
    {
        return FormGroups.Last();
    }

    /// <summary>
    /// Adds a form group to this column.
    /// </summary>
    /// <param name="newFormGroup">The form group configuration to add.</param>
    internal void AddFormGroup(FormGroupConfig newFormGroup)
    {
        FormGroups.Add(newFormGroup);
    }

    /// <summary>
    /// Removes all form groups from this column.
    /// </summary>
    internal void DeleteAllFormGroups()
    {
        FormGroups  = new List<FormGroupConfig>();
    }

    /// <summary>
    /// Returns all grid subfield configurations from all form groups in this column, optionally filtered by parent field name.
    /// </summary>
    /// <param name="fieldName">Optional parent field identifier filter. If empty or null, returns grid fields from all fields in all form groups.</param>
    /// <returns>A list of grid field configurations.</returns>
    internal List<FieldGridConfig> GetGridFieldList(string fieldName)
    {
        var returnList = new List<FieldGridConfig>();
        foreach (var formGroup in FormGroups)
        {
            returnList.AddRange(formGroup.GetGridFieldList(fieldName));
        };
        return returnList;
    }

    internal List<string> GetUIEvents()
    {
        var uiEvents = FormGroups.SelectMany(group => group.GetUIEvents()).ToList();
        return uiEvents;
    }

}

