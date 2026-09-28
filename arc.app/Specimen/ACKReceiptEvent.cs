using arc.app.Common;
using arc.app.Config;
using arc.common;
using arc.common.Models.Laboratory;
using arc.common.Models.Specimen;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Executes the acknowledgment receipt event for a specimen,
/// updating its state and persisting receipt details.
/// </summary>
internal class ACKReceiptEvent : IRun
{
    private readonly ISpecimenRepository _specimenRepository;
    private readonly IListViewConfigFactory _listViewConfigFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ACKReceiptEvent"/> class.
    /// </summary>
    /// <param name="specimenRepository">
    /// Repository for loading and persisting specimen data.
    /// </param>
    /// <param name="listViewConfigFactory">
    /// Factory for obtaining list view configurations.
    /// </param>
    public ACKReceiptEvent(
        ISpecimenRepository specimenRepository,
        IListViewConfigFactory listViewConfigFactory)
    {
        _specimenRepository = specimenRepository;
        _listViewConfigFactory = listViewConfigFactory;
    }

    /// <summary>
    /// Runs the ACK receipt processing logic: deserializes the event payload,
    /// cleans up test and culture type entries, retrieves view configuration,
    /// sets the new state, and executes the acknowledgment command.
    /// </summary>
    /// <param name="dataToSave">
    /// JSON string containing the ACK receipt event data.
    /// </param>
    /// <param name="Id">
    /// Identifier for the entity triggering this event (unused in this implementation).
    /// </param>
    /// <param name="command">
    /// Event model carrying state transition information.
    /// </param>
    /// <param name="eventData">
    /// Optional configuration for the event execution.
    /// </param>
    /// <returns>
    /// A task that resolves to an integer status code (0 on success).
    /// </returns>
    public async Task<int> RunAsync(
        string dataToSave,
        string Id,
        EventModel command,
        EventConfig eventData = null)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        var data = JsonConvert.DeserializeObject<ACKReceiptEventModel>(dataToSave, settings);

        data.Crafted
            .FirstOrDefault(t => t.Name == "testselectionpage")
            ?.Contents
            .Remove(
                data.Crafted
                    .FirstOrDefault(t => t.Name == "testselectionpage")
                    .Contents
                    .FirstOrDefault(item => item.Key == "XCategX")
            );

        data.Crafted
            .FirstOrDefault(t => t.Name == "culturetypeselectionpage")
            ?.Contents
            .Remove(
                data.Crafted
                    .FirstOrDefault(t => t.Name == "culturetypeselectionpage")
                    .Contents
                    .FirstOrDefault(item => item.Key == "XCategX")
            );

        var viewConfig = await _listViewConfigFactory.GetViewAsync(data.View);

        data.StateId = command.NewStateId;

        await _specimenRepository.AcknowledgeReceiptCommandAsync(
            data,
            new List<CultureTestConfigModel>(),
            dataToSave
        );

        return 0;
    }
}
