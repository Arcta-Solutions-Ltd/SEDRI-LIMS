using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Handles adding an expert rule condition from the record view embedded list.
/// </summary>
internal class AddExpertRuleConditionEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddExpertRuleConditionEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    public AddExpertRuleConditionEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Inserts a condition row mapped from the add form payload.
    /// </summary>
    /// <param name="dataToSave">JSON form payload; <c>Id</c> is the parent expert rule id.</param>
    /// <param name="id">Unused for add.</param>
    /// <param name="command">Posted event model.</param>
    /// <param name="eventData">Event configuration metadata.</param>
    /// <returns>The new condition id.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var row = ExpertRuleConditionFormMapper.ToDataModelFromJson(dataToSave, isAdd: true);

        logWriter.LogInfo(
            $"Add expert rule condition expertRuleId={row.ExpertRuleId} startVal={row.StartVal} endVal={row.EndVal}",
            nameof(AddExpertRuleConditionEvent),
            nameof(RunAsync));

        return await expertRuleRepository.AddExpertRuleConditionAsync(row);
    }
}
