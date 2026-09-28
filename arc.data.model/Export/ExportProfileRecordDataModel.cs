namespace arc.data.model.Export
{
    /// <summary>
    /// Represents the fields in the exportprofilerecord table in the database.
    /// </summary>
    public class ExportProfileRecordDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets foreign key linking the exportprofilerecord table to the exportprofile table.
        /// </summary>
        public int ExportProfileId { get; set; }
        /// <summary>
        /// Gets or sets the tablename that the data will be extracted from.
        /// </summary>
        public string? TableName { get; set; }
        /// <summary>
        /// Gets or sets the fieldname that the data will be extracted from.
        /// </summary>
        public string? FieldName { get; set; }
        /// <summary>
        /// Gets or sets the headername, which is the header for the column that will appear in the export file.
        /// </summary>
        public string? HeaderName { get; set; }
        public string? FormName { get; set; }
        public string? LabelName { get; set; }
        /// <summary>
        /// Gets or sets the order in which the field appears in the export file.
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets additional data in JSON format.
        /// </summary>
        [Jsonb]
        public string? MoreData { get; set; }
    }
}
