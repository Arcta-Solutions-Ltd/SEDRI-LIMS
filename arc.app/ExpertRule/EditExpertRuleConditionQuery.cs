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
/// Loads a single expert rule condition for the record-view edit form initial query.
/// </summary>
internal class EditExpertRuleConditionQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes the query runner with the application service provider.
    /// </summary>
    /// <param name="serviceProvider">Used to resolve repository and logging services.</param>
    public EditExpertRuleConditionQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the edit expert rule condition query and returns form-ready JSON with nullable measurement range values.
    /// </summary>
    /// <param name="queryFilter">Filter configuration containing the condition id.</param>
    /// <param name="token">Token context; not used by this query.</param>
    /// <returns>JSON form payload, or <c>{}</c> when the row is not found.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var id = queryFilter.GetIntegerValue("id");

        logWriter.LogInfo(
            $"Loading expert rule condition for edit id={id}",
            nameof(EditExpertRuleConditionQuery),
            nameof(RunAsync));

        var row = await expertRuleRepository.EditExpertRuleConditionByIdQueryAsync(queryFilter);
        if (row == null || row.Id <= 0)
        {
            logWriter.LogInfo(
                $"Expert rule condition not found for edit id={id}",
                nameof(EditExpertRuleConditionQuery),
                nameof(RunAsync));
            return "{}";
        }

        var formModel = ExpertRuleConditionFormMapper.ToEditFormModel(row);
        logWriter.LogInfo(
            $"Expert rule condition edit load id={row.Id} expertRuleId={row.ExpertRuleId} startVal={row.StartVal} endVal={row.EndVal}",
            nameof(EditExpertRuleConditionQuery),
            nameof(RunAsync));

        return JsonConvert.SerializeObject(formModel);
    }
}
