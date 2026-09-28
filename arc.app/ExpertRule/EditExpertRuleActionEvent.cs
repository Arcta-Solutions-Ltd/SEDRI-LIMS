using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Handles editing an expert rule action from the record view embedded list.
/// </summary>
internal class EditExpertRuleActionEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditExpertRuleActionEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    public EditExpertRuleActionEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Updates an action row mapped from the edit form payload.
    /// </summary>
    /// <param name="dataToSave">JSON form payload.</param>
    /// <param name="id">Expert rule action id.</param>
    /// <param name="command">Posted event model.</param>
    /// <param name="eventData">Event configuration metadata.</param>
    /// <returns>The updated action id.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var row = ExpertRuleActionFormMapper.ToDataModelFromJson(dataToSave, isAdd: false);

        if (row.Id <= 0 && int.TryParse(id, out var parsedId))
        {
            row.Id = parsedId;
        }

        AddExpertRuleActionEvent.LogIncomingDualTarget(logWriter, dataToSave, row.Id);

        logWriter.LogInfo(
            $"Edit expert rule action id={row.Id} antibioticId={row.AntibioticId} antibioticGroupId={row.AntibioticGroupId}",
            nameof(EditExpertRuleActionEvent),
            nameof(RunAsync));

        return await expertRuleRepository.EditExpertRuleActionAsync(row);
    }
}
