using arc.app.Coding;
using arc.app.Common;
using arc.common.Models;
using arc.common.Models.Coding;
using arc.common.Models.Laboratory;
using arc.data.Common;
using arc.data.model.Organism;
using arc.data.Utils;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    /// <summary>
    /// Repository for organism (and taxonomy) data access. Provides list, lookup, hierarchy,
    /// dropdown, search, synonym, and culture-count operations for the organism/coding area.
    /// </summary>
    /// <param name="sqlQuery">Query executor for select operations.</param>
    /// <param name="logWriter">Logger for repository operations.</param>
    /// <param name="sqlCommand">Command executor for update/insert/delete.</param>
    public class OrganismRepository(ISqlQuery sqlQuery, ILogWriter logWriter, ISqlCommand sqlCommand) : GeneralRepository(sqlQuery, logWriter, sqlCommand), IOrganismRepository
    {
        /// <summary>
        /// Returns the organism list for the organism listview (with optional filters).
        /// </summary>
        /// <param name="queryFilters">Filters for the list (e.g. search, taxonomy).</param>
        /// <returns>Sequence of organism list models for grid display.</returns>
        public async Task<IEnumerable<OrganismListModel>> GetOrganismListAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get organism list query", "OrganismRepository", "GetOrganismListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrganismListQuery(), "Get Organism List", queryFilters);
        }

        /// <summary>
        /// Returns a single organism list entry by ID (e.g. for edit form or record view).
        /// </summary>
        /// <param name="queryFilters">Filter containing the organism/list entry Id.</param>
        /// <returns>The organism list model, or default if not found.</returns>
        public async Task<OrganismListModel> GetOrganismListEntrByIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get organism list entry by id query", "OrganismRepository", "GetOrganismListEntrByIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrganismListEntryByIdQuery(), "Get Organism List Entry", queryFilters);
        }

        /// <summary>
        /// Returns a single organism by ID.
        /// </summary>
        /// <param name="queryFilters">Filter containing the organism Id.</param>
        /// <returns>The organism list model, or default if not found.</returns>
        public async Task<OrganismListModel> OrganismByIdQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get organism by id query", "OrganismRepository", "OrganismByIdQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrganismByIdQuery(), "Get Single Organism By Id", queryFilters);
        }

        /// <summary>
        /// Returns a single organism by code.
        /// </summary>
        /// <param name="queryFilters">Filter containing the organism code.</param>
        /// <returns>The organism list model, or default if not found.</returns>
        public async Task<OrganismListModel> OrganismByCodeQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get organism by code query", "OrganismRepository", "OrganismByCodeQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrganismByCodeQuery(), "Get Single Organism By Code", queryFilters);
        }

        /// <summary>
        /// Returns the family list (taxonomy options), optionally filtered (e.g. by order).
        /// </summary>
        /// <param name="queryFilters">Filters for family options.</param>
        /// <returns>Sequence of option configs for dropdown/list.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetFamilyListAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get family list query", "OrganismRepository", "GetFamilyListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new FamilyListQuery(), "Get Family List", queryFilters);
        }

        /// <summary>
        /// Returns the order list (taxonomy options) for dropdowns.
        /// </summary>
        /// <returns>Sequence of option configs for order dropdown.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetOrderListAsync()
        {
            _logWriter.LogInfo("Run get order list query", "OrganismRepository", "GetOrderListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrderListQuery(), "Get Order List", new QueryFilterConfig());
        }

        /// <summary>
        /// Returns the genus list (taxonomy options), optionally filtered (e.g. by family).
        /// </summary>
        /// <param name="queryFilters">Filters for genus options.</param>
        /// <returns>Sequence of option configs for dropdown/list.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetGenusListAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get genus list query", "OrganismRepository", "GetGenusListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new GenusListQuery(), "Get Genus List", queryFilters);
        }

        /// <summary>
        /// Returns the species list (taxonomy options), optionally filtered (e.g. by genus).
        /// </summary>
        /// <param name="queryFilters">Filters for species options.</param>
        /// <returns>Sequence of option configs for dropdown/list.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetSpeciesListAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get species list query", "OrganismRepository", "GetSpeciesListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new SpeciesListQuery(), "Get Species List", queryFilters);
        }

        /// <summary>
        /// Returns the subspecies list (taxonomy options), optionally filtered (e.g. by species).
        /// </summary>
        /// <param name="queryFilters">Filters for subspecies options.</param>
        /// <returns>Sequence of option configs for dropdown/list.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetSubSpeciesListAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get sub species list query", "OrganismRepository", "GetSubSpeciesListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new SubSpeciesListQuery(), "Get Sub Species List", queryFilters);
        }

        /// <summary>
        /// Returns the serotype list (taxonomy options), optionally filtered.
        /// </summary>
        /// <param name="queryFilters">Filters for serotype options.</param>
        /// <returns>Sequence of option configs for dropdown/list.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetSerotypeListAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get serotype list query", "OrganismRepository", "GetSerotypeListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new SerotypeListQuery(), "Get Serotype List", queryFilters);
        }

        /// <summary>
        /// Returns organisms (as options) that are marked resistant, for filtering or reporting.
        /// </summary>
        /// <param name="queryFilters">Filters for resistant organism list.</param>
        /// <returns>Sequence of option configs.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetResistantOrganismQueryAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get resistant organism query", "OrganismRepository", "GetResistantOrganismQueryAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ResistantOrganismQuery(), "Get Resistant Organism List", queryFilters);
        }

        /// <summary>
        /// Searches organisms by name/code (or other criteria) and returns matching search models.
        /// </summary>
        /// <param name="queryFilters">Search criteria (e.g. text, filters).</param>
        /// <returns>Sequence of organism search models.</returns>
        public async Task<IEnumerable<OrganismSearchModel>> OrganismSearchAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run organism search query", "OrganismRepository", "OrganismSearchAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrganismSearchQuery(), "Organism Search", queryFilters);
        }

        /// <summary>
        /// Returns organism options for dropdown use, scoped to the user's laboratory.
        /// </summary>
        /// <param name="token">Token containing LaboratoryId for scope.</param>
        /// <returns>Sequence of option configs for organism dropdown.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetOrganismDropdownAsync(TokenInfoModel token)
        {
            _logWriter.LogInfo("Run get organism dropdown query", "OrganismRepository", "GetOrganismDropdownAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrganismDropDownQuery(), "Get Organism Dropdown List", new QueryFilterConfig().AddString("LaboratoryId", token.LaboratoryId));
        }

        /// <summary>
        /// Returns organism code options for dropdown use, scoped to the user's laboratory.
        /// </summary>
        /// <param name="token">Token containing LaboratoryId for scope.</param>
        /// <returns>Sequence of option configs for organism code dropdown.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetOrganismCodeDropdownAsync(TokenInfoModel token)
        {
            _logWriter.LogInfo("Run get organism code dropdown query", "OrganismRepository", "GetOrganismCodeDropdownAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrganismCodeDropDownQuery(), "Get Organism Code Dropdown List", new QueryFilterConfig().AddString("LaboratoryId", token.LaboratoryId));
        }

        /// <summary>
        /// Returns the taxonomy hierarchy (order, family, genus, etc.) for a given organism ID.
        /// </summary>
        /// <param name="organismId">The organism ID.</param>
        /// <returns>Hierarchy model with taxonomy levels for the organism.</returns>
        public async Task<OrganismHierarchyModel> GetOrganismHierarchyAsync(int organismId)
        {
            _logWriter.LogInfo("Run get organism hierarchy query", "OrganismRepository", "GetOrganismHierarchyAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new GetHierarchyFromOrganismId(), "Get Organism Hierarchy", new QueryFilterConfig().AddInteger("Id", organismId));
        }

        /// <summary>
        /// Returns a human-readable description for an organism scope (taxonomy or organism group).
        /// Used for displaying organism scope in laboratory config list views.
        /// </summary>
        /// <param name="orderId">Order taxonomy ID.</param>
        /// <param name="familyId">Family taxonomy ID.</param>
        /// <param name="genusId">Genus taxonomy ID.</param>
        /// <param name="speciesId">Species taxonomy ID.</param>
        /// <param name="subspeciesId">Subspecies taxonomy ID.</param>
        /// <param name="serotypeId">Serotype taxonomy ID.</param>
        /// <param name="orgGroupCodingId">Organism group coding ID.</param>
        /// <param name="organismId">Specific organism ID.</param>
        /// <returns>Display string for the organism scope.</returns>
        public async Task<string> GetOrganismScopeDescriptionAsync(int orderId, int familyId, int genusId, int speciesId, int subspeciesId, int serotypeId, int orgGroupCodingId, int organismId)
        {
            _logWriter.LogInfo("Run get organism scope description query", "OrganismRepository", "GetOrganismScopeDescriptionAsync");
            var queryFilters = new QueryFilterConfig()
                .AddInteger("orderid", orderId)
                .AddInteger("familyid", familyId)
                .AddInteger("genusid", genusId)
                .AddInteger("speciesid", speciesId)
                .AddInteger("subspeciesid", subspeciesId)
                .AddInteger("serotypeid", serotypeId)
                .AddInteger("orggroupcodingid", orgGroupCodingId)
                .AddInteger("organismid", organismId);
            return await _sqlQuery.QueryReturningStringAsync(new GetOrganismScopeDescriptionQuery(), "Get Organism Scope Description", queryFilters);
        }

        /// <summary>
        /// Determines whether an organism (by ID or organism group) matches the given organism scope configuration.
        /// </summary>
        public async Task<bool> IsOrganismInScopeAsync(int organismId, int orgGroupCodingId, OrganismScopeCultureTestOptionModel scope)
        {
            if (scope == null) return false;

            if (scope.OrganismId > 0)
            {
                return organismId == scope.OrganismId;
            }

            if (scope.OrgGroupCodingId > 0)
            {
                return orgGroupCodingId == scope.OrgGroupCodingId;
            }

            if (organismId <= 0)
            {
                return false;
            }

            var hierarchy = await GetOrganismHierarchyAsync(organismId);
            if (hierarchy == null) return false;

            return (scope.OrderId == 0 || hierarchy.OrderId == scope.OrderId)
                && (scope.FamilyId == 0 || hierarchy.FamilyId == scope.FamilyId)
                && (scope.GenusId == 0 || hierarchy.GenusId == scope.GenusId)
                && (scope.SpeciesId == 0 || hierarchy.SpeciesId == scope.SpeciesId)
                && (scope.SubspeciesId == 0 || hierarchy.SubSpeciesId == scope.SubspeciesId)
                && (scope.SerotypeId == 0 || hierarchy.SerotypeId == scope.SerotypeId);
        }

        /// <summary>
        /// Returns order and family for a given genus ID (used in taxonomy cascades).
        /// </summary>
        /// <param name="queryFilters">Filter containing the genus Id.</param>
        /// <returns>Genus model with order and family populated.</returns>
        public async Task<GenusModel> GetOrderAndFamilyFromGenusIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get serotype list query", "OrganismRepository", "GetSerotypeListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new OrderAndFamilyFromGenusIdQuery(), "Get order and family from genus id", queryFilters);
        }

        /// <summary>
        /// Returns the list of synonyms for an organism (or filter).
        /// </summary>
        /// <param name="queryFilters">Filter (e.g. organism Id) for synonym list.</param>
        /// <returns>List of organism synonym data models.</returns>
        public async Task<List<OrganismSynonymDataModel>> GetOrganismSynonymListAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get organism synonym list query", "OrganismRepository", "GetOrganismSynonymListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new GetSynonymListQuery(), "Get Organism Synonym List", queryFilters);
        }

        /// <summary>
        /// Returns the count of cultures associated with an organism (e.g. for delete/usage checks).
        /// </summary>
        /// <param name="queryFilters">Filter containing the organism Id.</param>
        /// <returns>Number of cultures linked to the organism.</returns>
        public async Task<int> GetOrganismCultureCountAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run get organism culture countquery", "OrganismRepository", "GetOrganismCultureCountAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new OrganismCultureCountQuery(), "Get organism culture count", queryFilters);
        }

        /// <summary>
        /// Updates organism synonyms from the provided JSON payload.
        /// </summary>
        /// <param name="dataToSave">JSON containing synonym edit data (e.g. organism Id and synonym list).</param>
        public async Task EditSynonymsAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<EditSynonymModel>(dataToSave);
            _logWriter.LogInfo("Edit synonym command", "OrganismRepository", "EditSynonymsAsync");
            await _sqlCommand.CommandWithTypeQueryAsync(new SynonymCommand(), "Edit Synonyms", data);
        }
    }
}
