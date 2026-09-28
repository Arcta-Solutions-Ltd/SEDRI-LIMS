using arc.app.Config.Events;
using arc.app.Configuration;
using arc.common;
using arc.common.Models;
using arc.common.Models.Specimen;
using arc.common.Utils;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Common
{
    /// <summary>
    /// Handles batch events and processes them accordingly.
    /// </summary>
    public class BatchEventHandler : IBatchEventHandler
    {
        private readonly IWorkflowHandler _workflowHandler;
        private readonly IEventAdapter _eventAdapter;
        private readonly IHandleEvent _handleEvent;
        private readonly IJsonElementRemover _jsonElementRemover;
        private readonly IJsonReplacer _jsonReplacer;
        private readonly ILogWriter _logWriter;

        private string _validationMessage = "";

        /// <summary>
        /// Initializes a new instance of the BatchEventHandler class.
        /// </summary>
        /// <param name="workflowHandler">The workflow handler instance.</param>
        /// <param name="eventAdapter">The event adapter instance.</param>
        /// <param name="handleEvent">The event handler instance.</param>
        /// <param name="jsonElementRemover">The JSON element remover instance.</param>
        /// <param name="jsonReplacer">The JSON replacer instance.</param>
        /// <param name="logWriter">The log writer instance.</param>
        public BatchEventHandler(IWorkflowHandler workflowHandler, IEventAdapter eventAdapter, IHandleEvent handleEvent, IJsonElementRemover jsonElementRemover, IJsonReplacer jsonReplacer, ILogWriter logWriter)
        {
            _workflowHandler = workflowHandler;
            _eventAdapter = eventAdapter;
            _handleEvent = handleEvent;
            _jsonElementRemover = jsonElementRemover;
            _jsonReplacer = jsonReplacer;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Handles the batch event asynchronously by iterating selected items and delegating to the configured batch target event.
        /// </summary>
        /// <param name="message">The message containing event data as a JSON string.</param>
        /// <param name="token">The token information model.</param>
        public async Task Handle(string message, TokenInfoModel token)
        {
            var itemList = JsonConvert.DeserializeObject<BatchSelectedItemsModel>(message);

            message = _jsonElementRemover.RemoveElementsByValue(message, "selecteditems");

            var command = JsonConvert.DeserializeObject<EventModel>(message);
            var eventData = await _eventAdapter.GetEventAsync(command.Event);
            var targetEvent = await _eventAdapter.GetEventAsync(eventData.BatchEvent);

            _logWriter.LogInfo(
                $"Batch event {eventData.EventName} delegating to {eventData.BatchEvent} for {itemList.SelectedItems?.Count ?? 0} item(s)",
                nameof(BatchEventHandler),
                nameof(Handle));

            message = _jsonReplacer.ChangeValueInJsonString(message, "Event", eventData.BatchEvent);
            command.Event = eventData.BatchEvent;

            foreach (var item in itemList.SelectedItems)
            {
                var runEvent = true;
                message = _jsonReplacer.ChangeValueInJsonString(message, "Id", item.Id.ToString());

                if (!string.IsNullOrEmpty(eventData.BatchParentIdField))
                {
                    message = _jsonReplacer.ChangeValueInJsonString(message, eventData.BatchParentIdField, item.Id.ToString());
                    _logWriter.LogInfo(
                        $"Batch item {item.Id}: set {eventData.BatchParentIdField} for event {eventData.BatchEvent}",
                        nameof(BatchEventHandler),
                        nameof(Handle));
                }

                if (item.StateId > 0)
                {
                    var newCommand = await _workflowHandler.GetNextStateAsync(message, command, targetEvent, token);
                    runEvent = !(!string.IsNullOrEmpty(newCommand.StateId) && newCommand.NewStateId == "");
                    if (!runEvent)
                    {
                        _logWriter.LogInfo(
                            $"Batch item {item.Id}: skipped (wrong workflow state {item.StateId})",
                            nameof(BatchEventHandler),
                            nameof(Handle));
                    }
                }

                if (runEvent)
                {
                    _logWriter.LogInfo(
                        $"Batch item {item.Id}: running event {eventData.BatchEvent}",
                        nameof(BatchEventHandler),
                        nameof(Handle));

                    await _handleEvent.HandleAsync(message, token);
                    _validationMessage = _handleEvent.GetValidationMessage();
                    if (!string.IsNullOrEmpty(_validationMessage))
                    {
                        _logWriter.LogInfo(
                            $"Batch stopped at item {item.Id}: {_validationMessage}",
                            nameof(BatchEventHandler),
                            nameof(Handle));
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the validation message.
        /// </summary>
        /// <returns>The validation message as a string.</returns>
        public string GetValidationMessage()
        {
            return _validationMessage;
        }
    }

}
