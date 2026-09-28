using arc.common.ExtensionMethods;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace arc.app.Common.Display
{
    /// <inheritdoc />
    public class JsonDisplayRootResolver : IJsonDisplayRootResolver
    {
        private readonly ILogger<JsonDisplayRootResolver> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDisplayRootResolver"/> class.
        /// </summary>
        /// <param name="logger">Logger used to report unresolvable display roots.</param>
        public JsonDisplayRootResolver(ILogger<JsonDisplayRootResolver> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public string Resolve(string message, EventConfig eventDetails)
        {
            var displayRoot = eventDetails?.DisplayRoot;

            if (string.IsNullOrWhiteSpace(displayRoot) || string.IsNullOrWhiteSpace(message))
            {
                return message;
            }

            JToken payload;
            try
            {
                using var stringReader = new StringReader(message);
                using var jsonReader = new JsonTextReader(stringReader) { DateParseHandling = DateParseHandling.None };
                payload = JToken.ReadFrom(jsonReader);
            }
            catch (JsonReaderException exception)
            {
                _logger.LogWarning(
                    exception,
                    "Display root: payload for event {EventName} could not be parsed, falling back to the payload root",
                    eventDetails.EventName);
                return message;
            }

            var rootToken = payload.SelectTokenByPath(displayRoot);

            if (rootToken == null)
            {
                _logger.LogWarning(
                    "Display root: path '{DisplayRoot}' configured for event {EventName} does not resolve in the stored payload, so no fields will be displayed",
                    displayRoot,
                    eventDetails.EventName);
                return message;
            }

            if (rootToken.Type != JTokenType.Object)
            {
                _logger.LogWarning(
                    "Display root: path '{DisplayRoot}' configured for event {EventName} resolved to {TokenType} but an object is required, falling back to the payload root",
                    displayRoot,
                    eventDetails.EventName,
                    rootToken.Type);
                return message;
            }

            _logger.LogDebug(
                "Display root: resolved path '{DisplayRoot}' for event {EventName}",
                displayRoot,
                eventDetails.EventName);

            return rootToken.ToString(Formatting.None);
        }
    }
}
