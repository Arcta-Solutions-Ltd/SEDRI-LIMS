using arc.app.Common;
using arc.common;
using arc.common.Models.Coding;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using arc.data.model.Coding;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Handles adding an expert rule test condition from the record view embedded list.
/// </summary>
internal class AddExpertRuleTestConditionEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddExpertRuleTestConditionEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    public AddExpertRuleTestConditionEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Inserts a test condition row mapped from the add form payload.
    /// </summary>
    /// <param name="dataToSave">JSON form payload; <c>Id</c> is the parent expert rule id.</param>
    /// <param name="id">Unused for add.</param>
    /// <param name="command">Posted event model.</param>
    /// <param name="eventData">Event configuration metadata.</param>
    /// <returns>The new test condition id.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var payload = JsonConvert.DeserializeObject<ExpertRuleTestConditionEditFormModel>(dataToSave);
        ExpertRuleTestConditionDataModel row;
        try
        {
            row = ExpertRuleTestConditionFormMapper.ToDataModel(payload, isAdd: true);
        }
        catch (InvalidOperationException ex)
        {
            logWriter.LogInfo(
                $"Add expert rule test condition rejected expertRuleId={payload?.Id}: {ex.Message}",
                nameof(AddExpertRuleTestConditionEvent),
                nameof(RunAsync));
            throw;
        }

        logWriter.LogInfo(
            $"Add expert rule test condition expertRuleId={row.ExpertRuleId} testName={row.TestName} fieldName={row.FieldName}",
            nameof(AddExpertRuleTestConditionEvent),
            nameof(RunAsync));

        return await expertRuleRepository.AddExpertRuleTestConditionAsync(row);
    }
}
