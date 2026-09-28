using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Repository interface for export schedule data access.
    /// </summary>
    public interface IExportScheduleRepository
    {
        /// <summary>
        /// Gets the list of export schedules for a given export profile.
        /// </summary>
        /// <param name="queryFilters">Filter containing ExportProfileId.</param>
        /// <returns>List of schedule list models for the embedded list.</returns>
        Task<List<ExportScheduleListModel>> GetSchedulesByProfileIdAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Gets a single export schedule by ID for editing.
        /// </summary>
        /// <param name="queryFilters">Filter containing the schedule Id.</param>
        /// <returns>The export schedule model, or null if not found.</returns>
        Task<ExportScheduleModel?> GetByIdAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Gets all enabled export schedules for the background service to process.
        /// </summary>
        /// <returns>List of export schedule models.</returns>
        Task<List<ExportScheduleModel>> GetEnabledSchedulesAsync();

        /// <summary>
        /// Adds a new export schedule.
        /// </summary>
        /// <param name="dataToSave">JSON serialized ExportScheduleModel.</param>
        /// <returns>The ID of the newly created schedule.</returns>
        Task<int> AddExportScheduleAsync(string dataToSave);

        /// <summary>
        /// Updates an existing export schedule.
        /// </summary>
        /// <param name="dataToSave">JSON serialized ExportScheduleModel.</param>
        /// <returns>The ID of the updated schedule.</returns>
        Task<int> EditExportScheduleAsync(string dataToSave);

        /// <summary>
        /// Deletes an export schedule by ID.
        /// </summary>
        /// <param name="id">The schedule ID to delete.</param>
        Task DeleteExportScheduleAsync(string id);

        /// <summary>
        /// Gets the last run date for a given schedule (for incremental export).
        /// </summary>
        /// <param name="scheduleId">The schedule ID.</param>
        /// <returns>The last run date, or null if never run.</returns>
        Task<System.DateTime?> GetLastRunForScheduleAsync(int scheduleId);

        /// <summary>
        /// Checks whether a schedule with the given name already exists for the profile.
        /// </summary>
        /// <param name="exportProfileId">The export profile ID.</param>
        /// <param name="name">The schedule name (case-insensitive).</param>
        /// <param name="excludeScheduleId">Optional schedule ID to exclude (for edit).</param>
        /// <returns>True if a duplicate name exists.</returns>
        Task<bool> ScheduleNameExistsAsync(int exportProfileId, string name, int? excludeScheduleId = null);
    }
}
