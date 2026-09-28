using System;
using System.Collections.Generic;

namespace arc.common.Models.Export
{
    public class ExportRunRequestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ExportProfileId { get; set; }

        public List<string> SpecimenTypeIds { get; set; } = new List<string>();
        public List<string> SpecimenStateIds { get; set; } = new List<string>();
        public List<string> TagIds { get; set; } = new List<string>();

        public List<string> OrganisationIds { get; set; } = new List<string>();
        public List<string> LocationIds { get; set; } = new List<string>();

        public List<string> TestIds { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the organism IDs filter (comma-split from frontend).
        /// </summary>
        public List<string> OrganismIds { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets whether AST exclusive filter was applied.
        /// </summary>
        public string? ASTExclusive { get; set; }

        /// <summary>
        /// Gets or sets the file attachment identifier for the exported file (set after upload).
        /// </summary>
        public int? FileAttachmentId { get; set; }

        /// <summary>
        /// Gets or sets the export schedule ID when the run was triggered by a schedule (for incremental tracking).
        /// </summary>
        public int? ExportScheduleId { get; set; }
    }
}
