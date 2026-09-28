using System;

namespace arc.data.model.Export
{
    /// <summary>
    /// Data model for the exportschedule table.
    /// </summary>
    public class ExportScheduleDataModel : IdBase
    {
        /// <summary>
        /// Gets or sets the export profile identifier.
        /// </summary>
        public int ExportProfileId { get; set; }

        /// <summary>
        /// Gets or sets the schedule name (unique per profile).
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the filter criteria as JSONB.
        /// </summary>
        public string? Filter { get; set; }

        /// <summary>
        /// Gets or sets the frequency: hourly, daily, monthly.
        /// </summary>
        public required string Frequency { get; set; }

        /// <summary>
        /// Gets or sets the time of day for daily/monthly. Null for hourly.
        /// </summary>
        public TimeSpan? TimeOfDay { get; set; }

        /// <summary>
        /// Gets or sets the day of month for monthly (1-31). Null for hourly/daily.
        /// </summary>
        public int? DayOfMonth { get; set; }

        /// <summary>
        /// Gets or sets whether to only include new and modified records.
        /// </summary>
        public bool IncrementalOnly { get; set; }

        /// <summary>
        /// Gets or sets whether the schedule is enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the last modified date (maps to modifieddate column).
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}
