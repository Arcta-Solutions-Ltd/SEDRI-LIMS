namespace arc.common.Models.Config
{
    /// <summary>
    /// One selectable entry in the Add Existing Field picker.
    /// </summary>
    public class ExistingFieldOptionModel
    {
        /// <summary>
        /// Gets or sets the reference key <c>{sourceForm}|{sourcePage}|{fieldId}</c>.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the display text (translated field label).
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the field id, so the client can match without parsing <see cref="Key"/>.
        /// </summary>
        public string FieldId { get; set; }

        /// <summary>
        /// Gets or sets the field control type (singleline, combobox, fieldgrid, ...).
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// Gets or sets the resolved entity table the source page writes to, or null when unscoped.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the source form name id.
        /// </summary>
        public string SourceForm { get; set; }

        /// <summary>
        /// Gets or sets the source page name id.
        /// </summary>
        public string SourcePage { get; set; }

        /// <summary>
        /// Gets or sets the translated source page title, used for grouping in the UI.
        /// </summary>
        public string SourcePageTitle { get; set; }
    }
}
