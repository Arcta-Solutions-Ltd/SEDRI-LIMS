using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using arc.app.Common;

namespace arc.app.Specimen;

/// <summary>
/// Runs the Delete Culture workflow.
/// Delegates to <see cref="ICultureRepository.DeleteCultureAsync(string)"/> which performs
/// the database-side cascade deletes for the culture group determined by the provided Id.
/// </summary>
internal class DeleteCultureEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new instance of <see cref="DeleteCultureEvent"/>.
    /// </summary>
    /// <param name="serviceProvider">The application service provider used to resolve dependencies.</param>
    public DeleteCultureEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the delete culture operation for the specified Id.
    /// </summary>
    /// <param name="dataToSave">Unused for delete, kept for interface compatibility.</param>
    /// <param name="id">The culture Id whose parent group will be deleted.</param>
    /// <param name="command">The event model context.</param>
    /// <param name="eventData">Optional event config.</param>
    /// <returns>The deleted culture Id as an integer.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var breakpointRepository = _serviceProvider.GetService<ICultureRepository>();
        await breakpointRepository.DeleteCultureAsync(id);

        return int.Parse(id);
    }
}
