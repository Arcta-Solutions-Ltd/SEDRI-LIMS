using arc.app.Common;
using arc.common.Models;
using arc.common.Models.Coding;
using arc.common.Models.Laboratory;
using arc.data.model.Organism;
using arc.domain.Coding;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    public interface IOrganismRepository : IGeneralRepository
    {
        Task<IEnumerable<OrganismListModel>> GetOrganismListAsync(QueryFilterConfig parameters);
        Task<OrganismListModel> GetOrganismListEntrByIdAsync(QueryFilterConfig queryFilters);
        Task<OrganismListModel> OrganismByIdQueryAsync(QueryFilterConfig queryFilters);
        Task<IEnumerable<OptionsConfig>> GetOrderListAsync();
        Task<IEnumerable<OptionsConfig>> GetFamilyListAsync(QueryFilterConfig queryFilters);
        Task<IEnumerable<OptionsConfig>> GetGenusListAsync(QueryFilterConfig queryFilters);
        Task<IEnumerable<OptionsConfig>> GetSpeciesListAsync(QueryFilterConfig queryFilters);
        Task<IEnumerable<OptionsConfig>> GetSubSpeciesListAsync(QueryFilterConfig queryFilters);
        Task<IEnumerable<OptionsConfig>> GetSerotypeListAsync(QueryFilterConfig queryFilters);
        Task<IEnumerable<OrganismSearchModel>> OrganismSearchAsync(QueryFilterConfig queryFilters);
        Task<IEnumerable<OptionsConfig>> GetOrganismDropdownAsync(TokenInfoModel token);
        Task<IEnumerable<OptionsConfig>> GetOrganismCodeDropdownAsync(TokenInfoModel token);
        Task<OrganismHierarchyModel> GetOrganismHierarchyAsync(int organismId);
        Task<string> GetOrganismScopeDescriptionAsync(int orderId, int familyId, int genusId, int speciesId, int subspeciesId, int serotypeId, int orgGroupCodingId, int organismId);
        /// <summary>
        /// Determines whether an organism (by ID or organism group) matches the given organism scope configuration.
        /// Used for filtering isolate tests by organism scope on the AST screen.
        /// </summary>
        /// <param name="organismId">The organism ID (0 if using organism group).</param>
        /// <param name="orgGroupCodingId">The organism group coding ID (0 if using specific organism).</param>
        /// <param name="scope">The organism scope configuration to match against.</param>
        /// <returns>True if the organism matches the scope; otherwise false.</returns>
        Task<bool> IsOrganismInScopeAsync(int organismId, int orgGroupCodingId, OrganismScopeCultureTestOptionModel scope);
        Task<IEnumerable<OptionsConfig>> GetResistantOrganismQueryAsync(QueryFilterConfig queryFilters);
        Task<GenusModel> GetOrderAndFamilyFromGenusIdAsync(QueryFilterConfig queryFilters);
        Task<List<OrganismSynonymDataModel>> GetOrganismSynonymListAsync(QueryFilterConfig queryFilters);
        Task EditSynonymsAsync(string dataToSave);
        Task<int> GetOrganismCultureCountAsync(QueryFilterConfig queryFilters);
        Task<OrganismListModel> OrganismByCodeQueryAsync(QueryFilterConfig queryFilters);
    }
}
