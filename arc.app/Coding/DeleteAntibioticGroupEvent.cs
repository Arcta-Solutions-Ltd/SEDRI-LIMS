using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Coding;

/// <summary>
/// Event handler for deleting an antibiotic group.
/// </summary>
internal class DeleteAntibioticGroupEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAntibioticRepository _antibioticRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteAntibioticGroupEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    public DeleteAntibioticGroupEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _antibioticRepository = _serviceProvider.GetService<IAntibioticRepository>();
    }

    /// <summary>
    /// Executes the delete antibiotic group event asynchronously.
    /// </summary>
    /// <param name="dataToSave">Data to save, if applicable (not used in this implementation).</param>
    /// <param name="Id">The identifier of the antibiotic group to delete.</param>
    /// <param name="command">The event model containing additional context for the operation.</param>
    /// <param name="eventData">Optional configuration data for the event.</param>
    /// <returns>The identifier of the deleted antibiotic group as an integer.</returns>
    /// <remarks>
    /// This method interacts with the <see cref="IAntibioticRepository"/> to delete the specified antibiotic group.
    /// It parses the provided Id to ensure the return type matches the expected output.
    /// </remarks>
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        await _antibioticRepository.DeleteAntibioticGroupAsync(Id);

        return int.Parse(Id);
    }
}
