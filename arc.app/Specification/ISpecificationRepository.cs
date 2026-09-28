using arc.common.Models.Coding;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Specification
{
    /// <summary>
    /// Repository interface for specification data access.
    /// Handles specification list, options, counts, and CRUD operations.
    /// </summary>
    public interface ISpecificationRepository
    {
        /// <summary>
        /// Retrieves specification options for dropdown lists (e.g. breakpoint add/edit forms).
        /// </summary>
        /// <returns>List of options with Key = specification id, Text = composite display (Guidelines - Document (Version, Year)).</returns>
        Task<IEnumerable<OptionsConfig>> GetSpecificationOptionsForListAsync();

        /// <summary>
        /// Retrieves the specification display text for each of the supplied breakpoint ids, so a stored
        /// breakpoint id can be shown as the specification the user chose rather than as a number.
        /// </summary>
        /// <param name="breakpointIds">Breakpoint ids to resolve; non-positive and duplicate ids are ignored.</param>
        /// <returns>Breakpoint id to composite specification display text (Guidelines - Document (Version, Year)).</returns>
        Task<Dictionary<int, string>> GetSpecificationTextByBreakpointIdsAsync(IEnumerable<int> breakpointIds);

        /// <summary>
        /// Counts specifications that reference the given document type (listitem) id.
        /// Used when deleting a document type to validate no specifications depend on it.
        /// </summary>
        Task<int> GetSpecificationCountByDocumentIdAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Counts specifications that reference the given version number (listitem) id.
        /// Used when deleting a version to validate no specifications depend on it.
        /// </summary>
        Task<int> GetSpecificationCountByVersionNumberIdAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Counts specifications that reference the given publication year (listitem) id.
        /// Used when deleting a publication year to validate no specifications depend on it.
        /// </summary>
        Task<int> GetSpecificationCountByPublicationYearIdAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Retrieves a single specification by ID for the specification list view (e.g. edit or delete forms).
        /// </summary>
        Task<SpecificationListModel> GetSingleSpecificationForSpecificationListAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Retrieves the specification list with optional filters for the specification list view.
        /// </summary>
        Task<List<SpecificationListModel>> GetSpecificationListAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Counts specifications matching the given guidelines, document, version number, and publication year.
        /// Used by the specificationexists validation to prevent duplicate specifications.
        /// </summary>
        Task<int> GetSpecificationCountAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Counts how many expert rules, breakpoints, and alerts reference a given specification.
        /// Used to prevent deletion of specifications that are in use.
        /// </summary>
        Task<int> GetSpecificationUseCountAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Retrieves a single specification by ID for the delete specification flow.
        /// Returns the specification data before deletion for confirmation display.
        /// </summary>
        Task<SpecificationListModel> DeleteSpecificationAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Retrieves a single specification by ID for the edit specification form.
        /// </summary>
        Task<SpecificationListModel> EditSpecificationAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Updates an existing specification row in the database.
        /// </summary>
        /// <param name="dataToSave">JSON-serialized specification data.</param>
        Task EditSpecificationEventAsync(string dataToSave);
    }
}
