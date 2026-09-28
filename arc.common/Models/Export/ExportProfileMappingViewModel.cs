using System.Collections.Generic;

namespace arc.common.Models.Export
{
    /// <summary>
    /// Payload returned to the front-end when the Manage Mapping page opens.
    /// Carries the existing mapping (if any) plus the field options the editor needs
    /// in order to enforce constraints (e.g. specimen array requires a patient field).
    /// </summary>
    public class ExportProfileMappingViewModel
    {
        /// <summary>
        /// Gets or sets the export profile identifier the mapping belongs to.
        /// </summary>
        public int ExportProfileId { get; set; }

        /// <summary>
        /// Gets or sets the persisted mapping format ("json" or "xml"). Defaults to "json" when no mapping has been saved yet.
        /// </summary>
        public string Format { get; set; } = "json";

        /// <summary>
        /// Gets or sets the canonical structure tree as a JSON string. Empty/default for a new mapping.
        /// </summary>
        public string Structure { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the profile already has any field with TableName = patient.
        /// Used by the editor to enable the "specimens" array option.
        /// </summary>
        public bool HasPatientFields { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the profile already has any field with TableName = culture or culturetests.
        /// Used by the editor to enable the "cultures/isolates" array option.
        /// </summary>
        public bool HasCultureFields { get; set; }

        /// <summary>
        /// Gets or sets the classified field options derived from the profile's saved fields.
        /// </summary>
        public List<ExportProfileMappingFieldOption> FieldOptions { get; set; } = new();
    }

    /// <summary>
    /// Single profile field option exposed to the mapping editor.
    /// </summary>
    public class ExportProfileMappingFieldOption
    {
        /// <summary>
        /// Gets or sets the canonical key in the format "{fieldId}|{formName}|{tableName}|{description}".
        /// Matches the key produced by ExportProfileConfigHandler.GetFieldsAsync.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user-facing label (translated where applicable).
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the field identifier (the FieldName column from exportprofilerecord).
        /// </summary>
        public string FieldId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the form name the field belongs to.
        /// </summary>
        public string FormName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source table name (e.g. specimen, patient, culture, tests, culturetests, custom).
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the field category used by the editor: patient | specimen | culture | culturetest | directtest | custom.
        /// </summary>
        public string Category { get; set; } = "specimen";

        /// <summary>
        /// Gets or sets a value indicating whether this field is a grid field whose contents must be placed in their own array.
        /// </summary>
        public bool IsGridField { get; set; }

        /// <summary>
        /// Gets or sets the prepopulated grid sub-attribute definitions when <see cref="IsGridField"/> is true.
        /// </summary>
        public List<ExportProfileMappingGridSubField> GridSubFields { get; set; } = new();

        /// <summary>
        /// Gets or sets the canonical <see cref="Key"/> of the parent grid option this option was flattened
        /// from. Populated only on synthesised flattened-sub-field options that allow a single grid column to
        /// be used as a regular attribute outside its grid array. Null/empty for ordinary profile fields and
        /// for the parent grid options themselves.
        /// </summary>
        public string ParentGridKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the grid sub-field identifier (matches <see cref="ExportProfileMappingGridSubField.Id"/>)
        /// when this option is a flattened sub-field. Used by the export-time logic to resolve the column back
        /// to the parent grid. Null/empty for ordinary profile fields and for parent grid options.
        /// </summary>
        public string GridSubFieldId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Single grid sub-attribute that is auto-added when a grid field is converted into a mapping array.
    /// </summary>
    public class ExportProfileMappingGridSubField
    {
        /// <summary>
        /// Gets or sets the grid sub-field identifier (matches FieldGridConfig.Id on the form).
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user-facing label for the grid sub-field.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the underlying control type (combobox, dropdown, hierarchicalpicker, singleline, etc.).
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }

    /// <summary>
    /// Save payload posted by the Manage Mapping page.
    /// </summary>
    public class SaveExportProfileMappingRequest
    {
        /// <summary>
        /// Gets or sets the export profile identifier the mapping belongs to.
        /// </summary>
        public int ExportProfileId { get; set; }

        /// <summary>
        /// Gets or sets the rendering format ("json" or "xml").
        /// </summary>
        public string Format { get; set; } = "json";

        /// <summary>
        /// Gets or sets the canonical structure tree (JSON string).
        /// </summary>
        public string Structure { get; set; } = string.Empty;
    }
}
