using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Specimen;
/// <summary>
/// Runs the Delete Isolate workflow.
/// Delegates to <see cref="ICultureRepository.DeleteIsolateAsync(string)"/> which performs
/// the database-side deletes for the isolate group determined by the provided Id.
/// </summary>
internal class DeleteIsolateEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new instance of <see cref="DeleteIsolateEvent"/>.
    /// </summary>
    /// <param name="serviceProvider">The application service provider used to resolve dependencies.</param>
    public DeleteIsolateEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the delete isolate operation for the specified Id.
    /// </summary>
    /// <param name="dataToSave">Unused for delete, kept for interface compatibility.</param>
    /// <param name="id">The isolate's culture Id whose rows will be deleted.</param>
    /// <param name="command">The event model context.</param>
    /// <param name="eventData">Optional event config.</param>
    /// <returns>The deleted Id as an integer.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var breakpointRepository = _serviceProvider.GetService<ICultureRepository>();
        await breakpointRepository.DeleteIsolateAsync(id);

        return int.Parse(id);
    }
}
