using arc.app.Common;
using arc.app.Security;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Laboratory;

/// <summary>
/// Event handler for deleting a laboratory record.
/// </summary>
internal class DeleteLaboratoryEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILaboratoryRepository _laboratoryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLaboratoryEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    public DeleteLaboratoryEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _laboratoryRepository = _serviceProvider.GetService<ILaboratoryRepository>();
    }

    /// <summary>
    /// Executes the delete laboratory event asynchronously.
    /// </summary>
    /// <param name="dataToSave">Data to save, if applicable (not used in this implementation).</param>
    /// <param name="Id">The identifier of the laboratory record to delete.</param>
    /// <param name="command">The event model containing additional context for the operation.</param>
    /// <param name="eventData">Optional configuration data for the event.</param>
    /// <returns>The identifier of the deleted laboratory record as an integer.</returns>
    /// <remarks>
    /// This method interacts with the <see cref="ILaboratoryRepository"/> to delete the specified laboratory record.
    /// It parses the provided Id to ensure the return type matches the expected output.
    /// </remarks>
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        await _laboratoryRepository.DeleteLaboratoryAsync(Id);

        return int.Parse(Id);
    }
}
