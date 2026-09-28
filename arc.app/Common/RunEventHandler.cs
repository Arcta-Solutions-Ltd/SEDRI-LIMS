using arc.common;
using Newtonsoft.Json;
using System.Threading.Tasks;
using arc.app.Config.Events;
using arc.common.Models;

namespace arc.app.Common
{
    /// <summary>
    /// Handles the execution of events.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RunEventHandler"/> class.
    /// </remarks>
    /// <param name="eventAdapter">The event adapter.</param>
    /// <param name="batchEventHandler">The batch event handler.</param>
    /// <param name="logWriter">The log writer.</param>
    /// <param name="eventHandler">The event handler.</param>
    /// <param name="languageHandler">The handler to manage language.</param>
    public class RunEventHandler(IEventAdapter eventAdapter, IBatchEventHandler batchEventHandler, ILogWriter logWriter, IHandleEvent eventHandler, ILanguageHandler languageHandler) : IRunEventHandler
    {
        private readonly IEventAdapter _eventAdapter = eventAdapter;
        private readonly IBatchEventHandler _batchEventHandler = batchEventHandler;
        protected readonly ILogWriter _logWriter = logWriter;
        private readonly IHandleEvent _eventHandler = eventHandler;
        private readonly ILanguageHandler _languageHandler = languageHandler;

        /// <summary>
        /// Gets the result of the event.
        /// </summary>
        public IdModel Result { get; private set; }

        /// <summary>
        /// Runs the event handling process asynchronously.
        /// </summary>
        /// <param name="contents">The event content as a JSON string.</param>
        /// <param name="token">The token information model.</param>
        /// <returns>The validation message.</returns>
        public async Task<string> RunAsync(string contents, TokenInfoModel token)
        {
            var eventDetails = JsonConvert.DeserializeObject<EventModel>(contents);

            var eventData = await _eventAdapter.GetEventAsync(eventDetails.Event);

            var validationMessage = "";

            if (eventData.EventType.ToLower() == "batch")
            {
                // Handle batch events using the batch event handler.
                await _batchEventHandler.Handle(contents, token);
                validationMessage = _batchEventHandler.GetValidationMessage();
            }
            else
            {
                // Log the handling of non-batch events.
                _logWriter.LogInfo("Handle Event", nameof(RunEventHandler), nameof(RunAsync));

                // Handle non-batch events using the event handler.
                Result = await _eventHandler.HandleAsync(contents, token);

                // Log the retrieval of the validation message.
                _logWriter.LogInfo("Get Validation Message", nameof(RunEventHandler), nameof(RunAsync));
                validationMessage = _eventHandler.GetValidationMessage();
            }

            if (! string.IsNullOrEmpty(validationMessage))
            {
                validationMessage = await _languageHandler.TranslateAsync(validationMessage, token.LanguageId);
            }

            return validationMessage;
        }
    }
}
