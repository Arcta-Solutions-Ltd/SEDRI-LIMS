using System.Collections.Generic;

namespace arc.common.Models.Instruments.CustomImport
{
    /// <summary>
    /// Outcome of a Custom-interface record save. On failure <see cref="ErrorTag"/> holds a language tag and
    /// <see cref="ErrorDescription"/> a human-readable description for the resulting instrument error.
    /// </summary>
    public class CustomImportSaveResult
    {
        /// <summary>True when the whole record was saved and committed.</summary>
        public bool Success { get; set; }

        /// <summary>Id of the patient created or updated (0 when no patient was involved).</summary>
        public int PatientId { get; set; }

        /// <summary>Ids of the specimens created or updated.</summary>
        public List<int> SpecimenIds { get; set; } = new();

        /// <summary>Count of rows created across all levels.</summary>
        public int CreatedCount { get; set; }

        /// <summary>Count of rows updated across all levels.</summary>
        public int UpdatedCount { get; set; }

        /// <summary>Language tag describing the failure (empty on success).</summary>
        public string ErrorTag { get; set; } = string.Empty;

        /// <summary>Human-readable failure description (empty on success).</summary>
        public string ErrorDescription { get; set; } = string.Empty;
    }
}
