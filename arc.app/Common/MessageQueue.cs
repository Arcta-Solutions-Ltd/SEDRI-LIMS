using arc.app.AST;
using arc.app.Patient;
using arc.app.Specimen;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.Specimen;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace arc.app.Common
{

    /// <summary>
    /// Represents the message queue for handling messages.
    /// </summary>
    public class MessageQueue : IMessageQueue
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IListRepository _listRepository;
        private readonly ISpecimenRepository _specimenRepository;
        private readonly ICultureRepository _cultureRepository;
        private readonly IASTRepository _astRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueue"/> class.
        /// </summary>
        /// <param name="messageRepository">The message repository.</param>
        /// <param name="listRepository">The list repository.</param>
        /// <param name="specimenRepository">The specimen repository.</param>
        /// <param name="cultureRepository">The culture repository.</param>
        /// <param name="astRepository">The AST repository.</param>
        /// <param name="patientRepository">The patient repository.</param>
        /// <param name="logger">The logger for logging information.</param>
        public MessageQueue(IMessageRepository messageRepository, IListRepository listRepository, ISpecimenRepository specimenRepository, ICultureRepository cultureRepository,
            IASTRepository astRepository, IPatientRepository patientRepository, ILogger logger)
        {
            _messageRepository = messageRepository;
            _listRepository = listRepository;
            _specimenRepository = specimenRepository;
            _cultureRepository = cultureRepository;
            _astRepository = astRepository;
            _patientRepository = patientRepository;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new message asynchronously.
        /// </summary>
        /// <param name="message">The message content.</param>
        /// <param name="userName">The user name associated with the message.</param>
        /// <param name="eventData">The event configuration.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the added message.</returns>
        public async Task<int> AddAsync(string message, string userName, EventConfig eventData)
        {
            var normalizedMessage = message.NormalizeForQueueStorage();
            if (normalizedMessage != message)
            {
                _logger.LogInformation(
                    "MessageQueue: applied legacy quote normalization for event {EventName} (message length {MessageLength})",
                    eventData?.EventName,
                    message?.Length ?? 0);
            }
            else
            {
                _logger.LogInformation(
                    "MessageQueue: storing valid JSON unchanged for event {EventName} (message length {MessageLength})",
                    eventData?.EventName,
                    message?.Length ?? 0);
            }

            var formattedMessage = normalizedMessage;

            var topicId = await _listRepository.GetListIdFromValueAsync(eventData.Topic, 73);
            var eventId = await _listRepository.GetListIdFromValueAsync(eventData.EventName, 74, topicId);

            if (eventId < 1)
            {
                _logger.LogWarning("Missing event id for {eventName}", eventData.EventName);
            }
            else
            {
                _logger.LogDebug(
                    "MessageQueue: resolved event id {EventId} for {EventName} (topicId={TopicId})",
                    eventId,
                    eventData.EventName,
                    topicId);
            }

            var newMessage = new MessageModel
            {
                Message = formattedMessage,
                Username = userName,
                StatusId = 665,
                EventId = eventId,
                TopicId = topicId,
                TableName = NormalizeQueueTableName(eventData.TableName),
            };

            return await _messageRepository.AddAsync(newMessage);
        }

        /// <summary>
        /// Updates the status of a message asynchronously.
        /// </summary>
        /// <param name="id">The message ID.</param>
        /// <param name="newStatus">The new status.</param>
        /// <param name="recordId">The record ID associated with the status update.</param>
        /// <param name="table">The name of the table associated with the status update.</param>
        /// <param name="stateId">The state ID.</param>
        /// <param name="error">The error message, if any.</param>
        /// <param name="eventName">The event name associated with the status update.</param>
        /// <param name="data">The specimen patient model data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateStatusAsync(int id, int newStatus, int recordId, string table = "", int stateId = 0, string error = "", string eventName = "", SpecimenPatientModel data = null)
        {
            var specimenDetails = data == null || data.SpecimenId == 0 ? await SpecimenPatientDetailsAsync(table, eventName, recordId) : data;
            stateId = stateId == 0 && specimenDetails.StateId != 0 ? specimenDetails.StateId : stateId;

            await _messageRepository.UpdateStateAsync(id, newStatus, error, recordId, specimenDetails, stateId, NormalizeQueueTableName(table));
        }

        private static string NormalizeQueueTableName(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return null;
            }

            return tableName.Trim().ToLowerInvariant();
        }

        /// <summary>
        /// Resolves specimen and patient context after an event has been processed (alerts, queue status).
        /// The <paramref name="recordId"/> is the persisted row id (e.g. <c>specimencomment.id</c> after add),
        /// not a parent entity id from the event payload. Parent-entity remaps belong in
        /// <see cref="GetSpecimenPatientDetailsForWorkflowAsync"/>.
        /// </summary>
        /// <param name="table">The name of the table associated with the details.</param>
        /// <param name="eventName">The event name associated with the details.</param>
        /// <param name="recordId">The persisted record id returned from save or present in the payload on edit.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the specimen patient model.</returns>
        public async Task<SpecimenPatientModel> GetSpecimenPatientDetailsAsync(string table, string eventName, int recordId)
        {
            table = eventName.ToLower() == "addast" ? "culture" : table;
            table = eventName.ToLower() == "addspecimencomment" ? "specimen" : table;
            table = eventName.ToLower() == "addspecimentag" ? "specimentag" : table;
            table = eventName.ToLower() == "addpatienttag" ? "patienttag" : table;

            var result = await SpecimenPatientDetailsAsync(table, eventName, recordId);

            _logger.LogInformation(
                "Post-save context: event={EventName}, table={Table}, recordId={RecordId}, specimenId={SpecimenId}",
                eventName,
                table,
                recordId,
                result.SpecimenId);

            if (result.SpecimenId == 0 && IsCommentRelatedEvent(eventName))
            {
                _logger.LogWarning(
                    "Post-save context unresolved for comment event: event={EventName}, table={Table}, recordId={RecordId}",
                    eventName,
                    table,
                    recordId);
            }

            return result;
        }

        /// <summary>
        /// Resolves specimen and patient context for workflow state lookup before an event is processed.
        /// Remaps parent-entity ids on add events (e.g. culture id on culturecommentevent) to the correct table.
        /// </summary>
        /// <param name="eventDetails">The event configuration.</param>
        /// <param name="recordId">The record id from the event message (may be a parent entity id on add).</param>
        /// <returns>Specimen and patient context for workflow resolution.</returns>
        public async Task<SpecimenPatientModel> GetSpecimenPatientDetailsForWorkflowAsync(EventConfig eventDetails,  int recordId)
        {
            var table = eventDetails.TableName;
            var originalTable = table;
            table = eventDetails.EventName.IsSameAs("addculture") ? "specimen" : table;
            //table = eventDetails.EventName.IsSameAs("addisolateevent") ? "specimen" : table;
            table = eventDetails.EventName.IsSameAs("specimencomment") ? "specimen" : table;
            table = eventDetails.EventName.IsSameAs("culturecommentevent") ? "culture" : table;
            table = eventDetails.EventName.IsSameAs("addspecimentag") ? "specimentag" : table;
            table = eventDetails.EventName.IsSameAs("addpatienttag") ? "patienttag" : table;
            table = eventDetails.EventName.IsSameAs("updateast") ? "culture" : table;
            table = eventDetails.TableName.IsSameAs("tests") && eventDetails.EventType.IsSameAs("adddata") ? "specimen" : table;
            table = eventDetails.TableName.IsSameAs("culturetests") && eventDetails.EventType.IsSameAs("adddata") ? "culture" : table;

            if (!originalTable.IsSameAs(table) && eventDetails.EventName.IsSameAs("culturecommentevent"))
            {
                _logger.LogInformation(
                    "Workflow context remap: event={EventName}, originalTable={OriginalTable}, resolvedTable={ResolvedTable}, recordId={RecordId}",
                    eventDetails.EventName,
                    originalTable,
                    table,
                    recordId);
            }

            var result = await SpecimenPatientDetailsAsync(table, eventDetails.EventName, recordId);
            _logger.LogInformation(
                "Workflow context: event={EventName}, table={Table}, recordId={RecordId}, specimenId={SpecimenId}, patientId={PatientId}",
                eventDetails.EventName,
                table,
                recordId,
                result.SpecimenId,
                result.PatientId);

            return result;
        }

        private static bool IsCommentRelatedEvent(string eventName)
        {
            return eventName.IsSameAs("culturecommentevent")
                || eventName.IsSameAs("specimencomment")
                || eventName.IsSameAs("editcomment");
        }

        /// <summary>
        /// Looks up specimen and patient context for the given table and record id.
        /// Callers must pass the id type expected for <paramref name="table"/> (persisted row id for post-save,
        /// or parent entity id when invoked from workflow pre-save resolution).
        /// </summary>
        /// <param name="table">The resolved table name (may differ from event config after workflow remaps).</param>
        /// <param name="eventName">The event name associated with the lookup.</param>
        /// <param name="recordId">The record id to resolve.</param>
        /// <returns>The specimen and patient context, or an empty model when not found.</returns>
        private async Task<SpecimenPatientModel> SpecimenPatientDetailsAsync(string table, string eventName, int recordId)
        {

            var specimenDetails = new SpecimenPatientModel { StateId = 0, SpecimenId = 0 };
            if (!string.IsNullOrEmpty(table) && recordId > 0)

            {
                if (table.Equals("specimen", System.StringComparison.InvariantCultureIgnoreCase)) { specimenDetails = await _specimenRepository.GetSingleAsync(recordId); }
                if (table.Equals("culture", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    specimenDetails = await _cultureRepository.GetSingleAsync(recordId);
                }
                if (table.Equals("ast", System.StringComparison.InvariantCultureIgnoreCase)) { specimenDetails = await _astRepository.GetSpecimenAndPatientAsync(recordId); }
                if (table.Equals("specimencomment", System.StringComparison.InvariantCultureIgnoreCase)) { specimenDetails = await _specimenRepository.GetSingleCommentAsync(recordId); }
                if (table.Equals("specimentag", System.StringComparison.InvariantCultureIgnoreCase)) { specimenDetails = await _specimenRepository.GetSingleSpecimenTagAsync(recordId); }
                if (table.Equals("patientcomment", System.StringComparison.InvariantCultureIgnoreCase)) { specimenDetails = await _patientRepository.GetSingleCommentAsync(recordId); }
                if (table.Equals("patienttag", System.StringComparison.InvariantCultureIgnoreCase)) { specimenDetails = await _patientRepository.GetSinglePatientTagAsync(recordId); }
                if (table.Equals("tests", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    if (eventName.Equals("testselection", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        specimenDetails = await _specimenRepository.GetSingleAsync(recordId);
                    }
                    else
                    {
                        specimenDetails = await _specimenRepository.GetSingleFromDirectTestsAsync(recordId);
                    }
                }
                if (table.Equals("culturetests", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    if (eventName.Equals("culturetestselection", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        specimenDetails = await _specimenRepository.GetCultureSingleAsync(recordId);
                    }
                    else
                    {
                        specimenDetails = await _specimenRepository.GetSingleFromCultureTestsAsync(recordId);
                    }
                }
            }

            return specimenDetails;
        }
    }
}

