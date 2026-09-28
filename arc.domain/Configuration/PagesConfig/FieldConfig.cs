using System.Collections.Generic;
using Newtonsoft.Json;
using arc.domain.Configuration.ListsConfig;

namespace arc.domain.Configuration.PagesConfig;

/// <summary>
/// Represents configuration information for a field within a form or UI.
/// </summary>
public class FieldConfig
{
    /// <summary>
    /// Gets or sets the unique identifier of the field.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the field (e.g., text, number, date).
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the label text for the field.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field supports multiple selections.
    /// </summary>
    public bool MultiSelect { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field is required.
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed value or length for the field.
    /// </summary>
    public string Max { get; set; }

    /// <summary>
    /// Gets or sets the minimum allowed value or length for the field.
    /// </summary>
    public string Min { get; set; }

    /// <summary>
    /// Gets or sets the maximum decimal places allowed.
    /// </summary>
    public string MaxDPs { get; set; }

    /// <summary>
    /// Gets or sets the step interval for numeric inputs.
    /// </summary>
    public string Step { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field should default to the current date/time.
    /// </summary>
    public bool DefaultToNow { get; set; }

    /// <summary>
    /// Gets or sets the placeholder text displayed in the input.
    /// </summary>
    public string Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the name of the options set used by the field.
    /// </summary>
    public string OptionsName { get; set; }

    /// <summary>
    /// Gets or sets inline options defined directly on the field (used when <see cref="OptionsName"/> is not set).
    /// Keys are stored verbatim (e.g. 'Yes', 'No', '' for unset) and text values may be language tags
    /// (e.g. '@GenNon@') that are translated client-side. Enables a small fixed option set without a backing list.
    /// </summary>
    public List<OptionsConfig> Options { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether only leaf nodes are selectable in a hierarchical picker.
    /// When true, parent nodes (e.g. Growth, No Growth) are not selectable.
    /// </summary>
    [JsonProperty("leavesOnly")]
    public bool LeavesOnly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field should be highlighted.
    /// </summary>
    public bool Highlight { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the underline style should be removed.
    /// </summary>
    public bool NoUnderline { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a blank option is allowed.
    /// </summary>
    public bool Blank { get; set; }

    /// <summary>
    /// Gets or sets the default value for the field.
    /// </summary>
    public string DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the name of a parent list for dynamic field relationships.
    /// </summary>
    public string ParentList { get; set; }

    /// <summary>
    /// Gets or sets the mask string used for formatting input.
    /// </summary>
    public string Mask { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field is dynamically generated.
    /// </summary>
    public bool Dynamic { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether fixed values should be removed.
    /// </summary>
    public bool RemoveFixed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field's options should be translated.
    /// </summary>
    public bool TranslateOptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether tab navigation is enabled for the field.
    /// </summary>
    public bool Tab { get; set; }

    /// <summary>
    /// Gets or sets the database table name associated with the field.
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Gets or sets the suffix appended to the field label.
    /// </summary>
    public string Suffix { get; set; }

    /// <summary>
    /// Gets or sets a custom parameter for additional configuration.
    /// </summary>
    public string Parameter1 { get; set; }

    /// <summary>
    /// Gets or sets a second custom parameter for advanced configuration.
    /// </summary>
    public string Parameter2 { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to remove the Add button from a grid view.
    /// </summary>
    public bool RemoveGridAddButton { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to remove the Delete button from a grid view.
    /// </summary>
    public bool RemoveGridDeleteButton { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include an inline form button in a grid.
    /// </summary>
    public bool IncludeGridFormButton { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether entries in a grid can be reordered.
    /// </summary>
    public bool CanMoveEntries { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include additional options data.
    /// </summary>
    public bool IncludeOptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field can be enabled or disabled conditionally.
    /// </summary>
    public bool CanEnable { get; set; }

    /// <summary>
    /// Gets or sets configuration details that are user-modifiable.
    /// </summary>
    public string Configurable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field displays its value without allowing entry.
    /// Used when a field definition is reused on a second page to show a value for reference, so
    /// that only one of the copies can write to the shared storage key.
    /// </summary>
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Gets or sets additional markup or templating details for the field.
    /// </summary>
    public string Markup { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an associated code value should be displayed.
    /// </summary>
    public bool DisplayCode { get; set; }

    /// <summary>
    /// Gets or sets the label for the code field.
    /// </summary>
    public string CodeLabel { get; set; }

    /// <summary>
    /// Gets or sets the placeholder text for the code field.
    /// </summary>
    public string CodePlaceholder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field is intended for comment entry.
    /// </summary>
    public bool IsComment { get; set; }

    /// <summary>
    /// Gets or sets the value inherited from a parent field or configuration.
    /// </summary>
    public string ParentValue { get; set; }

    /// <summary>
    /// Gets or sets the list of level configurations associated with this field.
    /// </summary>
    public List<LevelConfig> Levels { get; set; }

    /// <summary>
    /// Gets or sets the list of rule configurations that apply to this field.
    /// </summary>
    public List<RuleConfig> Rules { get; set; }

    /// <summary>
    /// Gets or sets the collection of grid configuration settings for subfields.
    /// </summary>
    public List<FieldGridConfig> GridFields { get; set; } = [];

    /// <summary>
    /// Gets or sets additional data or metadata related to the field.
    /// </summary>
    public string MoreData { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field can be dragged in the UI.
    /// </summary>
    public bool Draggable { get; set; } = false;

    /// <summary>
    /// Gets or sets the list of allowed content types for file uploads (MIME types like "image/png" or extensions like ".pdf").
    /// When provided, the UI and backend will restrict uploaded files to these types.
    /// </summary>
    public List<string> ContentTypes { get; set; }

    public string AddFormUIEvent { get; set; }

    /// <summary>
    /// When true on a hierarchicalpicker field with optionsName locationlist or organisationlist,
    /// enables in-place add actions (subject to UI event permission). Omit or false on search/filter contexts.
    /// </summary>
    [JsonProperty("allowAdd")]
    public bool? AllowAdd { get; set; }

    public string FormUIEvent { get; set; }
    public string onFinish { get; set; }
    public bool IncludeFirstLine { get; set; } = true;
    public string FieldFormat { get; set; }
    public bool Embedded { get; set; }

    /// <summary>
    /// When true on a dropdown, combobox, or radio field, an "Other" option is appended at runtime
    /// and a companion text field is shown when that option is selected.
    /// </summary>
    [JsonProperty("allowOther")]
    public bool AllowOther { get; set; }

    /// <summary>
    /// Synthetic list item key used for the runtime "Other" option. Defaults to <see cref="OtherOptionConstants.Key"/>.
    /// </summary>
    [JsonProperty("otherOptionKey")]
    public string OtherOptionKey { get; set; }

    /// <summary>
    /// When set on a companion singleline field, holds the parent list field id whose "Other"
    /// selection controls visibility of this field.
    /// </summary>
    [JsonProperty("otherDetailsFor")]
    public string OtherDetailsFor { get; set; }

}
