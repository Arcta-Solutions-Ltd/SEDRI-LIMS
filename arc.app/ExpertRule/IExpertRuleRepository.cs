using arc.common.Models.AST;
using arc.common.Models.Coding;
using arc.common.Models.Laboratory;
using arc.data.model.Coding;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.ExpertRule
{
    public interface IExpertRuleRepository
    {
        Task<int> AddExpertRuleAsync(arc.domain.Coding.ExpertRule dataToSave);
        Task DeleteExpertRuleAsync(string id);
        Task DeleteExpertRuleConditionAsync(string id);
        Task DeleteExpertRuleActionAsync(string id);
        Task DeleteExpertRuleTestConditionAsync(string id);
        Task<int> AddExpertRuleConditionAsync(ExpertRuleConditionDataModel dataToSave);
        Task<int> EditExpertRuleConditionAsync(ExpertRuleConditionDataModel dataToSave);
        Task<ExpertRuleConditionDataModel> EditExpertRuleConditionByIdQueryAsync(QueryFilterConfig queryFilters);
        Task<int> AddExpertRuleTestConditionAsync(ExpertRuleTestConditionDataModel dataToSave);
        Task<int> EditExpertRuleTestConditionAsync(ExpertRuleTestConditionDataModel dataToSave);
        Task<ExpertRuleTestConditionDataModel> EditExpertRuleTestConditionByIdQueryAsync(QueryFilterConfig queryFilters);
        Task<int> AddExpertRuleActionAsync(ExpertRuleActionDataModel dataToSave);
        Task<int> EditExpertRuleActionAsync(ExpertRuleActionDataModel dataToSave);
        Task<ExpertRuleActionDataModel> EditExpertRuleActionByIdQueryAsync(QueryFilterConfig queryFilters);
        Task<List<ExpertRuleActionReturnModel>> ExpertRulesForTestPatternQueryAsync(QueryFilterConfig queryFilters);
        Task<int> EditExpertRuleAsync(arc.domain.Coding.ExpertRule dataToSave);
        Task<arc.domain.Coding.ExpertRule> EditExpertRuleQueryAsync(QueryFilterConfig queryFilters);
        Task<List<ExpertRuleListModel>> ExpertRuleListQueryAsync(QueryFilterConfig queryFilters);
        Task<ExpertRuleListModel> SingleExpertRuleForExpertRuleListQueryAsync(QueryFilterConfig queryFilters);
        Task<ExpertRuleViewModel> ExpertRuleViewByIdQueryAsync(QueryFilterConfig queryFilters);
        Task<List<ExpertRuleConditionListModel>> ExpertRuleConditionListByExpertRuleIdQueryAsync(QueryFilterConfig queryFilters);
        Task<List<ExpertRuleTestConditionListModel>> ExpertRuleTestConditionListByExpertRuleIdQueryAsync(QueryFilterConfig queryFilters);
        Task<List<ExpertRuleActionListModel>> ExpertRuleActionListByExpertRuleIdQueryAsync(QueryFilterConfig queryFilters);
        Task<List<ExpertRuleActionReturnModel>> ExpertRuleActionsQueryAsync(QueryFilterConfig queryFilters);
        Task<int> AddExpertRuleApprovalAsync(ExpertRuleApproval data);
        Task<List<arc.domain.Coding.ExpertRule>> RetrieveExpertRulesAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Retrieves expert rule names for the supplied rule ids, so a stored rule id can be shown as its name.
        /// </summary>
        /// <param name="ids">Expert rule ids to resolve; non-positive and duplicate ids are ignored.</param>
        /// <returns>Expert rule id to rule name.</returns>
        Task<Dictionary<int, string>> GetExpertRuleNamesByIdsAsync(IEnumerable<int> ids);

        /// <summary>
        /// Loads laboratory antibiotic group selection for expert-rule filtering.
        /// </summary>
        /// <param name="laboratoryId">Laboratory id for the current culture.</param>
        Task<LaboratoryAntibioticGroupFilterInfo> GetLaboratoryAntibioticGroupFilterInfoAsync(int laboratoryId);

        /// <summary>
        /// Returns the set of <c>antibiotic.id</c> values allowed for AST expert-rule action display for a laboratory:
        /// the union of all antibiotics in the laboratory's configured antibiotic groups via <c>antibioticcoding</c>.
        /// </summary>
        /// <param name="laboratoryId">Laboratory id for the current culture.</param>
        /// <returns>
        /// <c>null</c> when the laboratory selected Master (all groups); empty when no groups configured;
        /// otherwise the allowed antibiotic id set.
        /// </returns>
        Task<HashSet<int>?> GetAllowedLaboratoryAntibioticIdsAsync(int laboratoryId);

        /// <summary>
        /// Returns <c>antibiotic.id</c> and group listitem id (<c>antibioticcoding.codingid</c>) for the given antibiotics.
        /// </summary>
        Task<List<AntibioticIdGroupPair>> GetAntibioticGroupIdsByAntibioticIdsAsync(IReadOnlyCollection<int> antibioticIds);

        /// <summary>
        /// Returns antibiotics whose <c>antibioticcoding.codingid</c> is in the given group listitem id set.
        /// </summary>
        Task<List<AntibioticIdGroupPair>> GetAntibioticIdsByGroupIdsAsync(IReadOnlyCollection<int> groupListItemIds);
    }
}
