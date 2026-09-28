using arc.app.Common;
using arc.common;
using arc.common.ExtensionMethods;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Handles adding an expert rule action from the record view embedded list.
/// </summary>
internal class AddExpertRuleActionEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddExpertRuleActionEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    public AddExpertRuleActionEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Inserts an action row mapped from the add form payload.
    /// </summary>
    /// <param name="dataToSave">JSON form payload; <c>Id</c> is the parent expert rule id.</param>
    /// <param name="id">Unused for add.</param>
    /// <param name="command">Posted event model.</param>
    /// <param name="eventData">Event configuration metadata.</param>
    /// <returns>The new action id.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();

        LogIncomingDualTarget(logWriter, dataToSave, actionId: null);

        var row = ExpertRuleActionFormMapper.ToDataModelFromJson(dataToSave, isAdd: true);

        logWriter.LogInfo(
            $"Add expert rule action expertRuleId={row.ExpertRuleId} antibioticId={row.AntibioticId} antibioticGroupId={row.AntibioticGroupId}",
            nameof(AddExpertRuleActionEvent),
            nameof(RunAsync));

        return await expertRuleRepository.AddExpertRuleActionAsync(row);
    }

    internal static void LogIncomingDualTarget(ILogWriter logWriter, string dataToSave, int? actionId)
    {
        var token = JObject.Parse(dataToSave ?? "{}");
        var rawAntibioticId = token.GetNullableInt32("antibioticid");
        var rawGroupId = token.GetNullableInt32("antibioticgroupid");

        if (rawAntibioticId is > 0 && rawGroupId is > 0)
        {
            var actionPart = actionId.HasValue ? $" action id={actionId.Value}" : string.Empty;
            logWriter.LogInfo(
                $"Expert rule action save payload had both antibioticid={rawAntibioticId.Value} and antibioticgroupid={rawGroupId.Value};{actionPart} normalized to group only.",
                nameof(AddExpertRuleActionEvent),
                nameof(LogIncomingDualTarget));
        }
    }
}
