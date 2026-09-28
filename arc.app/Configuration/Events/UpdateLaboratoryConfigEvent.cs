using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.data.model.Configuration;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events;

/// <summary>
/// Event for updating a laboratory configuration.
/// </summary>
internal class UpdateLaboratoryConfigEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLaboratoryConfigEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    public UpdateLaboratoryConfigEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the event to update a laboratory configuration in the database.
    /// </summary>
    /// <param name="dataToSave">The updated data to save to the configuration.</param>
    /// <param name="id">The identifier of the laboratory configuration to update.</param>
    /// <param name="command">The event model containing event-related details.</param>
    /// <param name="eventData">Optional event configuration providing additional context.</param>
    /// <returns>
    /// An integer result, where <c>0</c> indicates successful execution.
    /// </returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var laboratoryConfigRepository = _serviceProvider.GetService<ILaboratoryConfigRepository>();
        var laboratoryConfig = await laboratoryConfigRepository.GetByIdAsync<LaboratoryConfigsDataModel>("LaboratoryConfigs", int.Parse(id));

        laboratoryConfig.Contents = dataToSave;
        await laboratoryConfigRepository.UpdateAsync(laboratoryConfig, "LaboratoryConfigs", "id");

        if (laboratoryConfig.ConfigName == "organismscopeculturetestoption")
        {
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            logWriter?.LogInfo($"Updated organism scope culture test option: LaboratoryId={laboratoryConfig.LaboratoryId}, ConfigId={laboratoryConfig.Id}", "UpdateLaboratoryConfigEvent", "RunAsync");
        }

        return 0;
    }
}
