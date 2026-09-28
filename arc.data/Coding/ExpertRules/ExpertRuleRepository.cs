using arc.app.ExpertRule;
using arc.app.Common;
using arc.common.Models.AST;
using arc.common.Models.Coding;
using arc.common.Models.Laboratory;
using arc.common.Utils;
using arc.data.model.Coding;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arc.data.Coding.ExpertRules;

namespace arc.data.Coding;

public class ExpertRuleRepository : IExpertRuleRepository
{
    private readonly ISqlQuery _sqlQuery;
    private readonly ISqlCommand _sqlCommand;
    private readonly ILogWriter _logWriter;

    public ExpertRuleRepository(ISqlQuery sqlQuery, ISqlCommand sqlCommand, ILogWriter logWriter)
    {
        _sqlQuery = sqlQuery;
        _sqlCommand = sqlCommand;
        _logWriter = logWriter;
    }
    public async Task<int> AddExpertRuleAsync(ExpertRule dataToSave)
    {
        _logWriter.LogInfo("Run add expert rule command", "ExpertRuleRepository", "AddExpertRuleAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddExpertRuleCommand(), "Add Expert Rule", dataToSave);
    }

    public async Task DeleteExpertRuleAsync(string id)
    {
        _logWriter.LogInfo("Run delete expert rule command", "ExpertRuleRepository", "DeleteExpertRuleAsync");
        await _sqlCommand.CarryOutCommandAsync(new DeleteExpertRuleCommand(), "Delete Expert Rule", id);
    }

    /// <summary>
    /// Deletes a single expert rule condition row by id.
    /// </summary>
    /// <param name="id">Expert rule condition id.</param>
    public async Task DeleteExpertRuleConditionAsync(string id)
    {
        _logWriter.LogInfo($"Delete expert rule condition id={id}", "ExpertRuleRepository", "DeleteExpertRuleConditionAsync");
        await _sqlCommand.CarryOutCommandAsync(new DeleteExpertRuleConditionCommand(), "Delete Expert Rule Condition", id);
    }

    /// <summary>
    /// Deletes a single expert rule action row by id.
    /// </summary>
    /// <param name="id">Expert rule action id.</param>
    public async Task DeleteExpertRuleActionAsync(string id)
    {
        _logWriter.LogInfo($"Delete expert rule action id={id}", "ExpertRuleRepository", "DeleteExpertRuleActionAsync");
        await _sqlCommand.CarryOutCommandAsync(new DeleteExpertRuleActionCommand(), "Delete Expert Rule Action", id);
    }

    /// <summary>
    /// Deletes a single expert rule test condition row by id.
    /// </summary>
    /// <param name="id">Expert rule test condition id.</param>
    public async Task DeleteExpertRuleTestConditionAsync(string id)
    {
        _logWriter.LogInfo($"Delete expert rule test condition id={id}", "ExpertRuleRepository", "DeleteExpertRuleTestConditionAsync");
        await _sqlCommand.CarryOutCommandAsync(new DeleteExpertRuleTestConditionCommand(), "Delete Expert Rule Test Condition", id);
    }

    /// <summary>
    /// Loads a single expert rule condition by id for the record-view edit form.
    /// </summary>
    /// <param name="queryFilters">Filter config; expects integer "id".</param>
    /// <returns>The condition row, or an empty model when not found.</returns>
    public async Task<ExpertRuleConditionDataModel> EditExpertRuleConditionByIdQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run edit expert rule condition query", "ExpertRuleRepository", nameof(EditExpertRuleConditionByIdQueryAsync));
        return await _sqlQuery.QueryReturningTypeAsync(new EditExpertRuleConditionByIdQuery(), "Edit Expert Rule Condition", queryFilters);
    }

    /// <summary>
    /// Inserts a single expert rule condition row from the record-view add form.
    /// </summary>
    /// <param name="dataToSave">Row values to insert.</param>
    /// <returns>The new condition id.</returns>
    public async Task<int> AddExpertRuleConditionAsync(ExpertRuleConditionDataModel dataToSave)
    {
        _logWriter.LogInfo($"Run add expert rule condition expertRuleId={dataToSave?.ExpertRuleId}", "ExpertRuleRepository", nameof(AddExpertRuleConditionAsync));
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddExpertRuleConditionCommand(), "Add Expert Rule Condition", dataToSave);
    }

    /// <summary>
    /// Updates a single expert rule condition row from the record-view edit form.
    /// </summary>
    /// <param name="dataToSave">Updated row values including id.</param>
    /// <returns>The updated condition id.</returns>
    public async Task<int> EditExpertRuleConditionAsync(ExpertRuleConditionDataModel dataToSave)
    {
        _logWriter.LogInfo($"Run edit expert rule condition id={dataToSave?.Id}", "ExpertRuleRepository", nameof(EditExpertRuleConditionAsync));
        return await _sqlCommand.CommandWithTypeQueryAsync(new EditExpertRuleConditionCommand(), "Edit Expert Rule Condition", dataToSave);
    }

    /// <summary>
    /// Loads a single expert rule test condition by id for the record-view edit form.
    /// </summary>
    /// <param name="queryFilters">Filter config; expects integer "id".</param>
    /// <returns>The test condition row, or an empty model when not found.</returns>
    public async Task<ExpertRuleTestConditionDataModel> EditExpertRuleTestConditionByIdQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run edit expert rule test condition query", "ExpertRuleRepository", nameof(EditExpertRuleTestConditionByIdQueryAsync));
        return await _sqlQuery.QueryReturningTypeAsync(new EditExpertRuleTestConditionByIdQuery(), "Edit Expert Rule Test Condition", queryFilters);
    }

    /// <summary>
    /// Inserts a single expert rule test condition row from the record-view add form.
    /// </summary>
    /// <param name="dataToSave">Row values to insert.</param>
    /// <returns>The new test condition id.</returns>
    public async Task<int> AddExpertRuleTestConditionAsync(ExpertRuleTestConditionDataModel dataToSave)
    {
        _logWriter.LogInfo($"Run add expert rule test condition expertRuleId={dataToSave?.ExpertRuleId}", "ExpertRuleRepository", nameof(AddExpertRuleTestConditionAsync));
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddExpertRuleTestConditionCommand(), "Add Expert Rule Test Condition", dataToSave);
    }

    /// <summary>
    /// Updates a single expert rule test condition row from the record-view edit form.
    /// </summary>
    /// <param name="dataToSave">Updated row values including id.</param>
    /// <returns>The updated test condition id.</returns>
    public async Task<int> EditExpertRuleTestConditionAsync(ExpertRuleTestConditionDataModel dataToSave)
    {
        _logWriter.LogInfo($"Run edit expert rule test condition id={dataToSave?.Id}", "ExpertRuleRepository", nameof(EditExpertRuleTestConditionAsync));
        return await _sqlCommand.CommandWithTypeQueryAsync(new EditExpertRuleTestConditionCommand(), "Edit Expert Rule Test Condition", dataToSave);
    }

    /// <summary>
    /// Loads a single expert rule action by id for the record-view edit form.
    /// </summary>
    /// <param name="queryFilters">Filter config; expects integer "id".</param>
    /// <returns>The action row, or an empty model when not found.</returns>
    public async Task<ExpertRuleActionDataModel> EditExpertRuleActionByIdQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run edit expert rule action query", "ExpertRuleRepository", nameof(EditExpertRuleActionByIdQueryAsync));
        return await _sqlQuery.QueryReturningTypeAsync(new EditExpertRuleActionByIdQuery(), "Edit Expert Rule Action", queryFilters);
    }

    /// <summary>
    /// Inserts a single expert rule action row from the record-view add form.
    /// </summary>
    /// <param name="dataToSave">Row values to insert.</param>
    /// <returns>The new action id.</returns>
    public async Task<int> AddExpertRuleActionAsync(ExpertRuleActionDataModel dataToSave)
    {
        _logWriter.LogInfo($"Run add expert rule action expertRuleId={dataToSave?.ExpertRuleId}", "ExpertRuleRepository", nameof(AddExpertRuleActionAsync));
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddExpertRuleActionCommand(), "Add Expert Rule Action", dataToSave);
    }

    /// <summary>
    /// Updates a single expert rule action row from the record-view edit form.
    /// </summary>
    /// <param name="dataToSave">Updated row values including id.</param>
    /// <returns>The updated action id.</returns>
    public async Task<int> EditExpertRuleActionAsync(ExpertRuleActionDataModel dataToSave)
    {
        _logWriter.LogInfo($"Run edit expert rule action id={dataToSave?.Id}", "ExpertRuleRepository", nameof(EditExpertRuleActionAsync));
        return await _sqlCommand.CommandWithTypeQueryAsync(new EditExpertRuleActionCommand(), "Edit Expert Rule Action", dataToSave);
    }

    public async Task<List<ExpertRuleActionReturnModel>> ExpertRulesForTestPatternQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run expert rules for test pattern query", "ExpertRuleRepository", "ExpertRulesForTestPatternQueryAsync");
        var result = await _sqlQuery.QueryReturningTypeAsync(new ExpertRulesForTestPatternQuery(), "Expert Rules For Test Pattern Query", queryFilters);
        return await ApplyLaboratoryAntibioticGroupFilterAsync(queryFilters, result);
    }

    public async Task<int> EditExpertRuleAsync(ExpertRule dataToSave)
    {
        _logWriter.LogInfo("Run edit expert rule command", "ExpertRuleRepository", "EditExpertRuleAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new EditExpertRuleCommand(), "Edit Expert Rule", dataToSave);
    }

    public async Task<ExpertRule> EditExpertRuleQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run edit expert rule query", "ExpertRuleRepository", "EditExpertRuleQueryAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new EditExpertRuleQuery(), "Edit Expert Rule", queryFilters);
    }

    /// <summary>
    /// Retrieves expert rule names for the supplied rule ids, so a stored rule id can be shown as its name.
    /// </summary>
    /// <param name="ids">Expert rule ids to resolve; non-positive and duplicate ids are ignored.</param>
    /// <returns>Expert rule id to rule name.</returns>
    public async Task<Dictionary<int, string>> GetExpertRuleNamesByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids?.Where(id => id > 0).Distinct().ToList() ?? [];
        if (idList.Count == 0)
        {
            return new Dictionary<int, string>();
        }

        _logWriter.LogInfo("Run get expert rule names by ids query", "ExpertRuleRepository", "GetExpertRuleNamesByIdsAsync");
        var queryFilters = new QueryFilterConfig().AddString("ids", string.Join(",", idList));
        var rows = await _sqlQuery.QueryReturningTypeAsync(
            new ExpertRuleNamesByIdsQuery(),
            "Get Expert Rule Names By Ids",
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

    public async Task<List<ExpertRuleListModel>> ExpertRuleListQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run expert rule list query", "ExpertRuleRepository", "ExpertRuleListQueryAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ExpertRuleListQuery(), "Expert Rule List Query", queryFilters);
    }

    public async Task<ExpertRuleListModel> SingleExpertRuleForExpertRuleListQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run single expert rule for expert rule list query", "ExpertRuleRepository", "SingleExpertRuleForExpertRuleListQueryAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new SingleExpertRuleForExpertRuleListQuery(), "Single Expert Rule Query", queryFilters);
    }

    /// <summary>
    /// Loads a single expert rule by id for the expert rule record view.
    /// Returns display text for all list fields; multiselect specimen types as comma-separated strings.
    /// </summary>
    /// <param name="queryFilters">Filter config; expects an integer "id" for the expert rule.</param>
    /// <returns>The expert rule view model for display on the record view.</returns>
    public async Task<ExpertRuleViewModel> ExpertRuleViewByIdQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run expert rule view query", "ExpertRuleRepository", "ExpertRuleViewByIdQueryAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ExpertRuleViewByIdQuery(), "Expert Rule View Query", queryFilters);
    }

    /// <summary>
    /// Loads expert rule conditions for a given expert rule for the record view embedded list.
    /// </summary>
    /// <param name="queryFilters">Filter config; expects "ExpertRuleId" with the expert rule ID.</param>
    /// <returns>List of conditions with display text for all list fields.</returns>
    public async Task<List<ExpertRuleConditionListModel>> ExpertRuleConditionListByExpertRuleIdQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run expert rule condition list query", "ExpertRuleRepository", "ExpertRuleConditionListByExpertRuleIdQueryAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ExpertRuleConditionListByExpertRuleIdQuery(), "Expert Rule Condition List", queryFilters);
    }

    /// <summary>
    /// Loads expert rule test conditions for a given expert rule for the record view embedded list.
    /// </summary>
    /// <param name="queryFilters">Filter config; expects "ExpertRuleId" with the expert rule ID.</param>
    /// <returns>List of test conditions.</returns>
    public async Task<List<ExpertRuleTestConditionListModel>> ExpertRuleTestConditionListByExpertRuleIdQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run expert rule test condition list query", "ExpertRuleRepository", "ExpertRuleTestConditionListByExpertRuleIdQueryAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ExpertRuleTestConditionListByExpertRuleIdQuery(), "Expert Rule Test Condition List", queryFilters);
    }

    /// <summary>
    /// Loads expert rule actions for a given expert rule for the record view embedded list.
    /// </summary>
    /// <param name="queryFilters">Filter config; expects "ExpertRuleId" with the expert rule ID.</param>
    /// <returns>List of actions with display text for all list fields.</returns>
    public async Task<List<ExpertRuleActionListModel>> ExpertRuleActionListByExpertRuleIdQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run expert rule action list query", "ExpertRuleRepository", "ExpertRuleActionListByExpertRuleIdQueryAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ExpertRuleActionListByExpertRuleIdQuery(_logWriter), "Expert Rule Action List", queryFilters);
    }

    public async Task<List<ExpertRuleActionReturnModel>> ExpertRuleActionsQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run retrieve expert rule actions query", "ExpertRuleRepository", "RetrieveExpertRuleActionsQueryAsync");
        var result = await _sqlQuery.QueryReturningTypeAsync(new ExpertRuleActionsQuery(), "Expert Rule Actions Query", queryFilters);
        return await ApplyLaboratoryAntibioticGroupFilterAsync(queryFilters, result);
    }

    /// <summary>
    /// Keeps expert rule actions whose target antibiotic (or antibiotic group) is allowed for the laboratory's selected antibiotic groups.
    /// No filtering when <paramref name="queryFilters"/> omits LaboratoryId or the lab has no antibiotic groups configured.
    /// </summary>
    private async Task<List<ExpertRuleActionReturnModel>> ApplyLaboratoryAntibioticGroupFilterAsync(
        QueryFilterConfig queryFilters,
        List<ExpertRuleActionReturnModel> actions)
    {
        if (actions == null || actions.Count == 0)
        {
            return actions;
        }

        if (!queryFilters.TryParseIntegerValue("laboratoryid", out var labId) || labId <= 0)
        {
            return actions;
        }

        var labConfig = new QueryFilterConfig();
        labConfig.Parameters.Add(new QueryValuesConfig { Key = "id", Value = labId.ToString() });

        var info = await _sqlQuery.QueryReturningTypeAsync(new LaboratoryAntibioticGroupFilterInfoQuery(), "Laboratory antibiotic group filter info", labConfig);
        if (info == null || string.IsNullOrWhiteSpace(info.AntibioticGroupIdsRaw))
        {
            return actions;
        }

        if (AntibioticGroupListIds.IncludesMaster(info.AntibioticGroupIdsRaw))
        {
            return actions;
        }

        var allowed = BuildAllowedLaboratoryGroupListItemIds(info);

        var antibioticIds = actions.Where(a => a.AntibioticId > 0).Select(a => a.AntibioticId).Distinct().ToList();
        var antibioticToCodingIds = await BuildAntibioticToCodingIdsMapAsync(antibioticIds);

        return ExpertRuleLaboratoryAntibioticFilter.FilterByAllowedAntibioticGroups(
            actions, allowed, antibioticToCodingIds);
    }

    /// <inheritdoc />
    public async Task<LaboratoryAntibioticGroupFilterInfo> GetLaboratoryAntibioticGroupFilterInfoAsync(int laboratoryId)
    {
        if (laboratoryId <= 0)
        {
            return null;
        }

        var labConfig = new QueryFilterConfig();
        labConfig.Parameters.Add(new QueryValuesConfig { Key = "id", Value = laboratoryId.ToString() });
        return await _sqlQuery.QueryReturningTypeAsync(
            new LaboratoryAntibioticGroupFilterInfoQuery(),
            "Laboratory antibiotic group filter info",
            labConfig);
    }

    /// <inheritdoc />
    public async Task<HashSet<int>?> GetAllowedLaboratoryAntibioticIdsAsync(int laboratoryId)
    {
        var info = await GetLaboratoryAntibioticGroupFilterInfoAsync(laboratoryId);
        if (info == null || string.IsNullOrWhiteSpace(info.AntibioticGroupIdsRaw))
        {
            return [];
        }

        if (AntibioticGroupListIds.IncludesMaster(info.AntibioticGroupIdsRaw))
        {
            return null;
        }

        var allowedGroupIds = BuildAllowedLaboratoryGroupListItemIds(info);
        if (allowedGroupIds.Count == 0)
        {
            return [];
        }

        var memberPairs = await GetAntibioticIdsByGroupIdsAsync(allowedGroupIds);
        return memberPairs.Select(p => p.Id).ToHashSet();
    }

    /// <summary>
    /// Parses the laboratory's configured antibiotic group listitem ids, excluding Master.
    /// </summary>
    private static HashSet<int> BuildAllowedLaboratoryGroupListItemIds(LaboratoryAntibioticGroupFilterInfo labInfo)
    {
        if (string.IsNullOrWhiteSpace(labInfo?.AntibioticGroupIdsRaw))
        {
            return [];
        }

        return AntibioticGroupListIds.ParseStoredIds(labInfo.AntibioticGroupIdsRaw)
            .Where(id => id != AntibioticGroupListIds.MasterListItemId)
            .ToHashSet();
    }

    /// <inheritdoc />
    public async Task<List<AntibioticIdGroupPair>> GetAntibioticGroupIdsByAntibioticIdsAsync(IReadOnlyCollection<int> antibioticIds)
    {
        if (antibioticIds == null || antibioticIds.Count == 0)
        {
            return [];
        }

        var idsParam = new QueryFilterConfig();
        idsParam.Parameters.Add(new QueryValuesConfig { Key = "antibioticids", Value = string.Join(",", antibioticIds.Distinct()) });
        return await _sqlQuery.QueryReturningTypeAsync(
            new AntibioticGroupIdByAntibioticIdsQuery(),
            "Antibiotic group ids by antibiotic ids",
            idsParam);
    }

    /// <inheritdoc />
    public async Task<List<AntibioticIdGroupPair>> GetAntibioticIdsByGroupIdsAsync(IReadOnlyCollection<int> groupListItemIds)
    {
        if (groupListItemIds == null || groupListItemIds.Count == 0)
        {
            return [];
        }

        var idsParam = new QueryFilterConfig();
        idsParam.Parameters.Add(new QueryValuesConfig { Key = "groupids", Value = string.Join(",", groupListItemIds.Distinct()) });
        return await _sqlQuery.QueryReturningTypeAsync(
            new AntibioticIdsByGroupIdsQuery(),
            "Antibiotic ids by group listitem ids",
            idsParam);
    }

    private async Task<Dictionary<int, HashSet<int>>> BuildAntibioticToCodingIdsMapAsync(IReadOnlyCollection<int> antibioticIds)
    {
        var antibioticToCodingIds = new Dictionary<int, HashSet<int>>();
        if (antibioticIds == null || antibioticIds.Count == 0)
        {
            return antibioticToCodingIds;
        }

        var pairs = await GetAntibioticGroupIdsByAntibioticIdsAsync(antibioticIds);
        foreach (var p in pairs)
        {
            if (!antibioticToCodingIds.TryGetValue(p.Id, out var codingIds))
            {
                codingIds = [];
                antibioticToCodingIds[p.Id] = codingIds;
            }

            codingIds.Add(p.GroupId);
        }

        return antibioticToCodingIds;
    }

    /// <summary>
    /// Inserts a new expert rule approval/rejection record.
    /// When the status is Rejected, the parent expert rule's Enabled is set to 'No'.
    /// </summary>
    /// <param name="data">The approval record containing ExpertRuleId, CodingStatusId, and RecordedBy.</param>
    /// <returns>The ID of the newly created expert rule approval record.</returns>
    public async Task<int> AddExpertRuleApprovalAsync(ExpertRuleApproval data)
    {
        _logWriter.LogInfo("Run add expert rule approval command", "ExpertRuleRepository", "AddExpertRuleApprovalAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddExpertRuleApprovalCommand(), "Insert Expert Rule Approval", data, _logWriter);
    }

    public async Task<List<ExpertRule>> RetrieveExpertRulesAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run retrieve expert rule list query", "ExpertRuleRepository", "RetrieveExpertRulesAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new RetrieveExpertRulesQuery(), "Retrieve Expert Rules", queryFilters);
    }
}
