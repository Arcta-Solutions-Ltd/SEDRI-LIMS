using arc.app.Common;
using arc.app.Config.Events;
using arc.common;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Monitoring;

/// <summary>
/// Handles formatted JSON queries by retrieving, deserializing, and translating 
/// event-related data. This class implements the <see cref="IFormattedJsonQueryHandler"/> interface.
/// </summary>
public class FormattedJsonQueryHandler : IFormattedJsonQueryHandler
{
    private readonly IMessageRepository _messageRepository;
    private readonly IEventAdapter _eventAdapter;
    private readonly IJsonDataFormatter _jsonDataFormatter;
    private readonly ILogger<FormattedJsonQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormattedJsonQueryHandler"/> class.
    /// </summary>
    /// <param name="messageRepository">An implementation of <see cref="IMessageRepository"/> to handle message retrieval.</param>
    /// <param name="eventAdapter">An implementation of <see cref="IEventAdapter"/> to retrieve event details.</param>
    /// <param name="jsonDataFormatter">An implementation of <see cref="IJsonDataFormatter"/> to format JSON data.</param>
    /// <param name="logger">Logger used to trace which queue entry and event a diary panel was built from.</param>
    public FormattedJsonQueryHandler(
        IMessageRepository messageRepository,
        IEventAdapter eventAdapter,
        IJsonDataFormatter jsonDataFormatter,
        ILogger<FormattedJsonQueryHandler> logger)
    {
        _messageRepository = messageRepository;
        _eventAdapter = eventAdapter;
        _jsonDataFormatter = jsonDataFormatter;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves and processes event-related data using query filters.
    /// </summary>
    /// <param name="queryFilters">The configuration of query filters to specify the data retrieval criteria.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the formatted JSON string.
    /// </returns>
    public async Task<string> GetDataAsync(QueryFilterConfig queryFilters)
    {
        var queueId = int.Parse(queryFilters.Parameters[0].Value);
        var message = await _messageRepository.GetSingleAsync(queueId);

        EventModel thisEvent;
        try
        {
            thisEvent = JsonConvert.DeserializeObject<EventModel>(message.Message);
        }
        catch (JsonException exception)
        {
            _logger.LogWarning(
                exception,
                "Queue entry {QueueId} could not be read as an event, so no contents can be shown for it",
                queueId);
            return JsonConvert.SerializeObject(new List<object>());
        }

        if (string.IsNullOrWhiteSpace(thisEvent?.Event))
        {
            _logger.LogWarning(
                "Queue entry {QueueId} has no event name, so no contents can be shown for it",
                queueId);
            return JsonConvert.SerializeObject(new List<object>());
        }

        _logger.LogInformation(
            "Building formatted contents for queue entry {QueueId}, event {EventName}",
            queueId,
            thisEvent.Event);

        var eventDetails = await _eventAdapter.GetEventAsync(thisEvent.Event);

        return await _jsonDataFormatter.TranslateAsync(message.Message, eventDetails);
    }
}
