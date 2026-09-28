using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Exports;

/// <summary>
/// Event handler for deleting an export schedule.
/// </summary>
internal class DeleteExportScheduleEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteExportScheduleEvent"/> class.
    /// </summary>
    public DeleteExportScheduleEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var repository = _serviceProvider.GetService<IExportScheduleRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();

        await repository.DeleteExportScheduleAsync(Id);
        logWriter?.LogInfo($"Export schedule deleted: id={Id}", nameof(DeleteExportScheduleEvent), nameof(RunAsync));
        return int.Parse(Id);
    }
}
