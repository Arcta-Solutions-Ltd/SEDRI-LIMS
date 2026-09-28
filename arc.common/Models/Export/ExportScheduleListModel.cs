using System;

namespace arc.common.Models.Export
{
    /// <summary>
    /// List view model for export schedules in the embedded list on the export profile record view.
    /// </summary>
    public class ExportScheduleListModel
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
        /// Gets or sets the schedule name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the frequency display (hourly, daily, monthly).
        /// </summary>
        public string Frequency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time of day display for daily/monthly. Null for hourly.
        /// </summary>
        public string? TimeOfDay { get; set; }

        /// <summary>
        /// Gets or sets the day of month for monthly. Null for hourly/daily.
        /// </summary>
        public int? DayOfMonth { get; set; }

        /// <summary>
        /// Gets or sets whether the schedule is enabled (display as "Yes"/"No").
        /// </summary>
        public string? Enabled { get; set; }

        /// <summary>
        /// Gets or sets the date/time of the last run for this schedule.
        /// </summary>
        public DateTime? LastRunAt { get; set; }
    }
}
