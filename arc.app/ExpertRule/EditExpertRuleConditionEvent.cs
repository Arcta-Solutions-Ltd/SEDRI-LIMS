using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Handles editing an expert rule condition from the record view embedded list.
/// </summary>
internal class EditExpertRuleConditionEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditExpertRuleConditionEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    public EditExpertRuleConditionEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Updates a condition row mapped from the edit form payload.
    /// </summary>
    /// <param name="dataToSave">JSON form payload.</param>
    /// <param name="id">Expert rule condition id.</param>
    /// <param name="command">Posted event model.</param>
    /// <param name="eventData">Event configuration metadata.</param>
    /// <returns>The updated condition id.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var row = ExpertRuleConditionFormMapper.ToDataModelFromJson(dataToSave, isAdd: false);

        if (row.Id <= 0 && int.TryParse(id, out var parsedId))
        {
            row.Id = parsedId;
        }

        logWriter.LogInfo(
            $"Edit expert rule condition id={row.Id} startVal={row.StartVal} endVal={row.EndVal}",
            nameof(EditExpertRuleConditionEvent),
            nameof(RunAsync));

        return await expertRuleRepository.EditExpertRuleConditionAsync(row);
    }
}
