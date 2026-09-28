using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Handles deletion of a single expert rule action from the record view.
/// </summary>
internal class DeleteExpertRuleActionEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteExpertRuleActionEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    public DeleteExpertRuleActionEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Deletes the expert rule action identified by <paramref name="id"/>.
    /// </summary>
    /// <param name="dataToSave">Unused form payload.</param>
    /// <param name="id">Expert rule action id.</param>
    /// <param name="command">Posted event model.</param>
    /// <param name="eventData">Event configuration metadata.</param>
    /// <returns>The deleted action id.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        await expertRuleRepository.DeleteExpertRuleActionAsync(id);
        return int.Parse(id);
    }
}
