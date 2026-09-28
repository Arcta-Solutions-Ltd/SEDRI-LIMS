using System;

namespace arc.common.Models.Export
{
    /// <summary>
    /// Persistence-shaped model for an export profile mapping.
    /// Mirrors the columns of the exportprofilemapping table.
    /// </summary>
    public class ExportProfileMappingModel
    {
        /// <summary>
        /// Gets or sets the mapping row identifier (0 when a new mapping is being saved).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the export profile identifier the mapping belongs to.
        /// </summary>
        public int ExportProfileId { get; set; }

        /// <summary>
        /// Gets or sets the rendering format. Must be either "json" or "xml".
        /// </summary>
        public string Format { get; set; } = "json";

        /// <summary>
        /// Gets or sets the canonical structure tree as a JSON string.
        /// </summary>
        public string Structure { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}
