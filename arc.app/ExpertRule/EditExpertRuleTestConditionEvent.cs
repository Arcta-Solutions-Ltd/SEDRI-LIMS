using arc.app.Common;
using arc.common;
using arc.common.Models.Coding;
using arc.data.model.Coding;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Handles editing an expert rule test condition from the record view embedded list.
/// </summary>
internal class EditExpertRuleTestConditionEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditExpertRuleTestConditionEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    public EditExpertRuleTestConditionEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Updates a test condition row mapped from the edit form payload.
    /// </summary>
    /// <param name="dataToSave">JSON form payload including crafted <c>TestGrid</c> when present.</param>
    /// <param name="id">Expert rule test condition id.</param>
    /// <param name="command">Posted event model.</param>
    /// <param name="eventData">Event configuration metadata.</param>
    /// <returns>The updated test condition id.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var payload = JsonConvert.DeserializeObject<ExpertRuleTestConditionEditFormModel>(dataToSave);
        if (payload != null && payload.Id <= 0 && int.TryParse(id, out var parsedId))
        {
            payload.Id = parsedId;
        }

        ExpertRuleTestConditionDataModel row;
        try
        {
            row = ExpertRuleTestConditionFormMapper.ToDataModel(payload, isAdd: false);
        }
        catch (InvalidOperationException ex)
        {
            logWriter.LogInfo(
                $"Edit expert rule test condition rejected id={id}: {ex.Message}",
                nameof(EditExpertRuleTestConditionEvent),
                nameof(RunAsync));
            throw;
        }

        if (row.Id <= 0 && int.TryParse(id, out parsedId))
        {
            row.Id = parsedId;
        }

        logWriter.LogInfo(
            $"Edit expert rule test condition id={row.Id} testName={row.TestName} fieldName={row.FieldName}",
            nameof(EditExpertRuleTestConditionEvent),
            nameof(RunAsync));

        return await expertRuleRepository.EditExpertRuleTestConditionAsync(row);
    }
}
