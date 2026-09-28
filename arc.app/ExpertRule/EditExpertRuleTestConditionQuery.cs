using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Loads a single expert rule test condition for the record-view edit form initial query.
/// </summary>
internal class EditExpertRuleTestConditionQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes the query runner with the application service provider.
    /// </summary>
    /// <param name="serviceProvider">Used to resolve repository and logging services.</param>
    public EditExpertRuleTestConditionQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the edit expert rule test condition query and returns form-ready JSON including <c>TestGrid</c>.
    /// </summary>
    /// <param name="queryFilter">Filter configuration containing the test condition id.</param>
    /// <param name="token">Token context; not used by this query.</param>
    /// <returns>JSON form payload, or <c>{}</c> when the row is not found.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var id = queryFilter.GetIntegerValue("id");

        logWriter.LogInfo(
            $"Loading expert rule test condition for edit id={id}",
            nameof(EditExpertRuleTestConditionQuery),
            nameof(RunAsync));

        var row = await expertRuleRepository.EditExpertRuleTestConditionByIdQueryAsync(queryFilter);
        if (row == null || row.Id <= 0)
        {
            logWriter.LogInfo(
                $"Expert rule test condition not found for edit id={id}",
                nameof(EditExpertRuleTestConditionQuery),
                nameof(RunAsync));
            return "{}";
        }

        var formModel = ExpertRuleTestConditionFormMapper.ToEditFormModel(row);
        logWriter.LogInfo(
            $"Expert rule test condition edit load id={row.Id} expertRuleId={row.ExpertRuleId} testGridLines={formModel.TestGrid?.Count ?? 0}",
            nameof(EditExpertRuleTestConditionQuery),
            nameof(RunAsync));

        return JsonConvert.SerializeObject(formModel);
    }
}
