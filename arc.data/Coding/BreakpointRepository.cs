using arc.app.Coding;
using arc.app.Common;
using arc.common.Models;
using arc.common.Models.Coding;
using arc.common.Utils;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    /// <summary>
    /// Repository for breakpoint data access. Handles add, edit, delete, and query operations
    /// for breakpoints (organism–antibiotic susceptibility criteria) and related list/organism queries.
    /// </summary>
    public class BreakpointRepository : IBreakpointRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakpointRepository"/> class.
        /// </summary>
        /// <param name="sqlCommand">Command executor for insert/update/delete.</param>
        /// <param name="sqlQuery">Query executor for select operations.</param>
        /// <param name="logWriter">Logger for repository operations.</param>
        public BreakpointRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand;
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Inserts a new breakpoint using the provided JSON payload.
        /// </summary>
        /// <param name="dataToSave">JSON containing breakpoint and crafted taxonomy/organism fields.</param>
        /// <returns>The ID of the newly inserted breakpoint.</returns>
        public async Task<int> AddBreakpointAsync(string dataToSave)
        {
            var data = GetBreakpointData(dataToSave);
            data.Enabled = "No";
            _logWriter.LogInfo("Run add breakpoint command", "BreakpointRepository", "AddBreakpointAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddBreakpointCommand(), "Insert Breakpoint", data, _logWriter);
        }

        /// <summary>
        /// Updates an existing breakpoint using the provided JSON payload.
        /// </summary>
        /// <param name="dataToSave">JSON containing breakpoint and crafted taxonomy/organism fields.</param>
        /// <returns>The number of rows affected by the update.</returns>
        public async Task<int> EditBreakpointAsync(string dataToSave)
        {
            var data = GetBreakpointData(dataToSave);
            _logWriter.LogInfo("Run edit breakpoint command", "BreakpointRepository", "EditBreakpointAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new EditBreakpointCommand(), "Edit Breakpoint", data);
        }

        /// <summary>
        /// Retrieves a single breakpoint by ID for editing (e.g. edit form load).
        /// </summary>
        /// <param name="queryFilters">Filter containing the breakpoint Id.</param>
        /// <returns>The breakpoint entity, or null if not found.</returns>
        public async Task<Breakpoint> EditBreakpointQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run edit breakpoint query", "BreakpointRepository", "EditBreakpointQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new EditBreakpointQuery(), "Edit Breakpoint Query", queryFilters);
        }

        /// <summary>
        /// Retrieves a single breakpoint including organism taxonomy (order, family, genus, species, etc.).
        /// </summary>
        /// <param name="queryFilters">Filter containing the breakpoint Id.</param>
        /// <returns>The breakpoint with organism details, or null if not found.</returns>
        public async Task<Breakpoint> BreakpointWithOrganismQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run breakpoint with organism query", "BreakpointRepository", "BreakpointWithOrganismQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new BreakpointWithOrganismQuery(), "Breakpoint with organism query", queryFilters);
        }

        public async Task<BreakpointListModel> BreakpointViewByIdQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run breakpoint view query", "BreakpointRepository", "BreakpointViewByIdQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new BreakpointViewByIdQuery(), "Breakpoint view query", queryFilters);
        }

        /// <summary>
        /// Returns the list of breakpoint lines (resultline rows) for a given breakpoint, with susceptibility display text.
        /// </summary>
        /// <param name="queryFilters">Filter config; expects "BreakpointId" with the breakpoint ID.</param>
        /// <returns>List of breakpoint line rows for the record view embedded list.</returns>
        public async Task<List<BreakpointLineListModel>> BreakpointLineListByBreakpointIdQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run breakpoint line list query", "BreakpointRepository", "BreakpointLineListByBreakpointIdQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new BreakpointLineListByBreakpointIdQuery(), "Breakpoint Line List", queryFilters);
        }

        /// <summary>
        /// Returns the list of breakpoints for the breakpoint listview (with filters).
        /// </summary>
        /// <param name="queryFilters">Filters (e.g. organism id, test method, specification, host, antibiotic name).</param>
        /// <returns>List of breakpoint list models for grid display.</returns>
        public async Task<List<BreakpointListModel>> BreakpointListQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run breakpoint list query", "BreakpointRepository", "BreakpointListQueryAsync");
            if (queryFilters.Parameters?.Any(p => string.Equals(p.Key, "organismid", StringComparison.OrdinalIgnoreCase)) == true)
            {
                var organismFilterValue = queryFilters.GetStringValue("organismid");
                if (!string.IsNullOrWhiteSpace(organismFilterValue))
                {
                    _logWriter.LogInfo($"Breakpoint list query with organism filter: {organismFilterValue}", "BreakpointRepository", "BreakpointListQueryAsync");
                }
            }
            return await _sqlQuery.QueryReturningTypeAsync(new BreakpointListQuery(), "Breakpoint List Query", queryFilters);
        }

        /// <summary>
        /// Returns all breakpoints associated with a given organism (e.g. for AST or organism scope).
        /// </summary>
        /// <param name="parameters">Filter containing organism identifier(s).</param>
        /// <returns>List of breakpoint entities for the organism.</returns>
        public async Task<List<Breakpoint>> GetBreakpointsForOrganismAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("Run get breakpoints for organism query", "BreakpointRepository", "GetBreakpointsForOrganismAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new BreakpointsForOrganismQuery(), "Get Breakpoints for Organism", parameters);
        }

        /// <summary>
        /// Inserts a new breakpoint approval record.
        /// </summary>
        /// <param name="data">The breakpoint approval data (BreakpointId, CodingStatusId, RecordedBy).</param>
        /// <returns>The ID of the newly inserted breakpoint approval record.</returns>
        public async Task<int> AddBreakpointApprovalAsync(BreakpointApproval data)
        {
            _logWriter.LogInfo("Run add breakpoint approval command", "BreakpointRepository", "AddBreakpointApprovalAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddBreakpointApprovalCommand(), "Insert Breakpoint Approval", data, _logWriter);
        }

        /// <summary>
        /// Deletes a breakpoint by ID.
        /// </summary>
        /// <param name="id">The breakpoint ID to delete.</param>
        public async Task DeleteBreakpointAsync(string id)
        {
            _logWriter.LogInfo("Run delete breakpoint command", "BreakpointRepository", "DeleteBreakpointAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteBreakpointCommand(), "Delete Breakpoint", id);
        }

        /// <summary>
        /// Deserializes the save payload and maps crafted taxonomy fields (order, family, genus, species,
        /// subspecies, serotype, org group coding) onto the breakpoint entity.
        /// </summary>
        /// <param name="dataToSave">JSON containing breakpoint and crafted page contents.</param>
        /// <returns>A <see cref="Breakpoint"/> instance with taxonomy IDs populated from crafted contents.</returns>
        private Breakpoint GetBreakpointData(string dataToSave)
        {
            var crafted = ArcJson.Deserialize<CraftedPagesModel>(dataToSave);
            var data = ArcJson.Deserialize<Breakpoint>(dataToSave);
            foreach (var avPair in crafted.Crafted[0].Contents)
            {
                switch (avPair.Key)
                {
                    case "orderid":
                        data.OrderId = avPair.value != null ? int.Parse(avPair.value) : 0;
                        break;
                    case "familyid":
                        data.FamilyId = avPair.value != null ? int.Parse(avPair.value) : 0;
                        break;
                    case "genusid":
                        data.GenusId = avPair.value != null ? int.Parse(avPair.value) : 0;
                        break;
                    case "speciesid":
                        data.SpeciesId = avPair.value != null ? int.Parse(avPair.value) : 0;
                        break;
                    case "subspeciesid":
                        data.SubSpeciesId = avPair.value != null ? int.Parse(avPair.value) : 0;
                        break;
                    case "serotypeid":
                        data.SerotypeId = avPair.value != null ? int.Parse(avPair.value) : 0;
                        break;
                    case "orggroupcodingid":
                        data.OrgGroupCodingId = avPair.value != null ? int.Parse(avPair.value) : 0;
                        break;
                    default:
                        break;
                }
            }
            return data;
        }
    }
}
