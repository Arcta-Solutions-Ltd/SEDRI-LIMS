using System;
using System.Collections.Generic;

namespace arc.common.Models.Export
{
    /// <summary>
    /// Model for export schedule configuration.
    /// Contains filter criteria (excluding start/end dates) and schedule timing.
    /// </summary>
    public class ExportScheduleModel
    {
        /// <summary>
        /// Gets or sets the schedule identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the export profile identifier.
        /// </summary>
        public int ExportProfileId { get; set; }

        /// <summary>
        /// Gets or sets the unique name for the schedule within the profile.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the filter criteria as JSON (OrganismId, SpecimenTypeId, SpecimenStateId, TagId, OrganisationId, LocationId, TestId, ASTExclusive).
        /// </summary>
        public string? Filter { get; set; }

        /// <summary>
        /// Gets or sets the frequency: 'hourly', 'daily', or 'monthly'.
        /// </summary>
        public string Frequency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time of day for daily/monthly runs (e.g. "09:00"). Null for hourly.
        /// </summary>
        public TimeSpan? TimeOfDay { get; set; }

        /// <summary>
        /// Gets or sets the day of month for monthly runs (1-31). Null for hourly/daily.
        /// </summary>
        public int? DayOfMonth { get; set; }

        /// <summary>
        /// Gets or sets whether to only include new and modified records since the last export.
        /// </summary>
        public bool IncrementalOnly { get; set; }

        /// <summary>
        /// Gets or sets whether the schedule is enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the output directory for saving export files (relative to storage root). Null uses default.
        /// </summary>
        public string? OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets which changes to include: "newonly" or "newandmodified". Null means full export.
        /// </summary>
        public string? ChangesToInclude { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}
