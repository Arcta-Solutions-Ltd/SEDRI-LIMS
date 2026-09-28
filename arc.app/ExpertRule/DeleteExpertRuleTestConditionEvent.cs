using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Handles deletion of a single expert rule test condition from the record view.
/// </summary>
internal class DeleteExpertRuleTestConditionEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteExpertRuleTestConditionEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    public DeleteExpertRuleTestConditionEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Deletes the expert rule test condition identified by <paramref name="id"/>.
    /// </summary>
    /// <param name="dataToSave">Unused form payload.</param>
    /// <param name="id">Expert rule test condition id.</param>
    /// <param name="command">Posted event model.</param>
    /// <param name="eventData">Event configuration metadata.</param>
    /// <returns>The deleted test condition id.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        await expertRuleRepository.DeleteExpertRuleTestConditionAsync(id);
        return int.Parse(id);
    }
}
