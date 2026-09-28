namespace arc.data.model.Export
{
    /// <summary>
    /// Represents the fields in the exportprofile table in the database.
    /// </summary>
    public class ExportProfileDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets the name of the export profile.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the export profile.
        /// </summary>
        public required string Description { get; set; }
    }
}
