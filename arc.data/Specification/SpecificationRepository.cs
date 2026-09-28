using arc.app.Common;
using arc.app.Specification;
using arc.common.Models.Coding;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specification
{
    /// <summary>
    /// Repository for specification data access.
    /// Handles specification list, options, counts, and CRUD operations.
    /// </summary>
    public class SpecificationRepository : ISpecificationRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        public SpecificationRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Retrieves specification options for dropdown lists (e.g. breakpoint add/edit forms).
        /// </summary>
        /// <returns>List of options with Key = specification id, Text = composite display (Guidelines - Document (Version, Year)).</returns>
        public async Task<IEnumerable<OptionsConfig>> GetSpecificationOptionsForListAsync()
        {
            _logWriter.LogInfo("Run specification options for list query", "SpecificationRepository", "GetSpecificationOptionsForListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new SpecificationOptionsForListQuery(), "Specification Options For List", new QueryFilterConfig());
        }

        /// <summary>
        /// Retrieves the specification display text for each of the supplied breakpoint ids, so a stored
        /// breakpoint id can be shown as the specification the user chose rather than as a number.
        /// </summary>
        /// <param name="breakpointIds">Breakpoint ids to resolve; non-positive and duplicate ids are ignored.</param>
        /// <returns>Breakpoint id to composite specification display text (Guidelines - Document (Version, Year)).</returns>
        public async Task<Dictionary<int, string>> GetSpecificationTextByBreakpointIdsAsync(IEnumerable<int> breakpointIds)
        {
            var idList = breakpointIds?.Where(id => id > 0).Distinct().ToList() ?? [];
            if (idList.Count == 0)
            {
                return new Dictionary<int, string>();
            }

            _logWriter.LogInfo("Run specification text for breakpoint ids query", "SpecificationRepository", "GetSpecificationTextByBreakpointIdsAsync");
            var queryFilters = new QueryFilterConfig().AddString("ids", string.Join(",", idList));
            var rows = await _sqlQuery.QueryReturningTypeAsync(
                new SpecificationTextForBreakpointIdsQuery(),
                "Specification Text For Breakpoint Ids",
                queryFilters);

            var result = new Dictionary<int, string>();
            foreach (var row in rows ?? [])
            {
                if (int.TryParse(row.Key, out var id) && !result.ContainsKey(id))
                {
                    result[id] = row.Text;
                }
            }

            return result;
        }

        /// <summary>
        /// Counts specifications that reference the given document type (listitem) id.
        /// Used when deleting a document type to validate no specifications depend on it.
        /// </summary>
        public async Task<int> GetSpecificationCountByDocumentIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run specification count by document id query", "SpecificationRepository", "GetSpecificationCountByDocumentIdAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new SpecificationCountByDocumentIdQuery(), "Specification Count By Document Id", queryFilters);
        }

        /// <summary>
        /// Counts specifications that reference the given version number (listitem) id.
        /// Used when deleting a version to validate no specifications depend on it.
        /// </summary>
        public async Task<int> GetSpecificationCountByVersionNumberIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run specification count by version number id query", "SpecificationRepository", "GetSpecificationCountByVersionNumberIdAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new SpecificationCountByVersionNumberIdQuery(), "Specification Count By Version Number Id", queryFilters);
        }

        /// <summary>
        /// Counts specifications that reference the given publication year (listitem) id.
        /// Used when deleting a publication year to validate no specifications depend on it.
        /// </summary>
        public async Task<int> GetSpecificationCountByPublicationYearIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run specification count by publication year id query", "SpecificationRepository", "GetSpecificationCountByPublicationYearIdAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new SpecificationCountByPublicationYearIdQuery(), "Specification Count By Publication Year Id", queryFilters);
        }

        /// <summary>
        /// Retrieves a single specification by ID for the specification list view (e.g. edit or delete forms).
        /// </summary>
        public async Task<SpecificationListModel> GetSingleSpecificationForSpecificationListAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run single specification for specification list query", "SpecificationRepository", "GetSingleSpecificationForSpecificationListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new SingleSpecificationForSpecificationListQuery(), "Single Specification Query", queryFilters);
        }

        /// <summary>
        /// Retrieves the specification list with optional filters for the specification list view.
        /// </summary>
        public async Task<List<SpecificationListModel>> GetSpecificationListAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get specification list query", "SpecificationRepository", "GetSpecificationListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new SpecificationListQuery(), "Get Specification List", queryFilters);
        }

        /// <summary>
        /// Counts specifications matching the given guidelines, document, version number, and publication year.
        /// Used by the specificationexists validation to prevent duplicate specifications.
        /// </summary>
        public async Task<int> GetSpecificationCountAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get specification count query", "SpecificationRepository", "GetSpecificationCountAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new SpecificationCountQuery(), "Get Specification Count", queryFilters);
        }

        /// <summary>
        /// Counts how many expert rules, breakpoints, and alerts reference a given specification.
        /// Used to prevent deletion of specifications that are in use.
        /// </summary>
        public async Task<int> GetSpecificationUseCountAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get specification use count query", "SpecificationRepository", "GetSpecificationUseCountAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new SpecificationUseCountQuery(), "Get Specification Use Count", queryFilters);
        }

        /// <summary>
        /// Retrieves a single specification by ID for the delete specification flow.
        /// Returns the specification data before deletion for confirmation display.
        /// </summary>
        public async Task<SpecificationListModel> DeleteSpecificationAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run delete specification query", "SpecificationRepository", "DeleteSpecificationAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new DeleteSpecificationQuery(), "Delete Specification Query", queryFilters);
        }

        /// <summary>
        /// Retrieves a single specification by ID for the edit specification form.
        /// </summary>
        public async Task<SpecificationListModel> EditSpecificationAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run edit specification query", "SpecificationRepository", "EditSpecificationAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new EditSpecificationQuery(), "Edit Specification Query", queryFilters);
        }

        /// <summary>
        /// Updates an existing specification row in the database.
        /// </summary>
        /// <param name="dataToSave">JSON-serialized specification data.</param>
        public async Task EditSpecificationEventAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<SpecificationListModel>(dataToSave);
            _logWriter.LogInfo("Run edit specification command", "SpecificationRepository", "EditSpecificationEventAsync");
            await _sqlCommand.CommandWithTypeQueryAsync(new EditSpecificationCommand(), "Edit Specification Event", data);
        }
    }
}
