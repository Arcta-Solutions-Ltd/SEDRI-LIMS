using System;

namespace arc.data.model.Export
{
    /// <summary>
    /// Data model for the exportprofilemapping table. Stores the user-defined JSON or XML
    /// structural mapper for a single export profile (one row per profile).
    /// </summary>
    public class ExportProfileMappingDataModel : IdBase
    {
        /// <summary>
        /// Gets or sets the export profile identifier the mapping belongs to.
        /// </summary>
        public int ExportProfileId { get; set; }

        /// <summary>
        /// Gets or sets the format the mapping is rendered in. Allowed values: "json" or "xml".
        /// </summary>
        public required string Format { get; set; }

        /// <summary>
        /// Gets or sets the canonical structure tree (JSON-encoded). Persisted as a JSONB column.
        /// </summary>
        [Jsonb]
        public required string Structure { get; set; }

        /// <summary>
        /// Gets or sets the last modified date (maps to the modifieddate column).
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}
