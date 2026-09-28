using arc.app.Common.Display;
using arc.common.ExtensionMethods;
using arc.common.Models.Monitoring;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Common
{
    /// <summary>
    /// Enriches stored JSON test/results structures using event display configuration (translations, list resolution, date trimming).
    /// </summary>
    public class JsonDataFormatter : IJsonDataFormatter
    {
        private readonly IJsonDisplayRootResolver _displayRootResolver;
        private readonly IJsonDisplaySectionBuilder _sectionBuilder;
        private readonly IDisplayValueResolverFactory _resolverFactory;
        private readonly ILogger<JsonDataFormatter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataFormatter"/> class.
        /// </summary>
        /// <param name="displayRootResolver">Narrows a crafted payload down to the object the display rows describe.</param>
        /// <param name="sectionBuilder">Filters and orders the payload against the display rows and sections.</param>
        /// <param name="resolverFactory">Supplies the resolver that turns a stored id into display text.</param>
        /// <param name="logger">Logger used to diagnose empty or partially matched panels on installed sites.</param>
        public JsonDataFormatter(
            IJsonDisplayRootResolver displayRootResolver,
            IJsonDisplaySectionBuilder sectionBuilder,
            IDisplayValueResolverFactory resolverFactory,
            ILogger<JsonDataFormatter> logger)
        {
            _displayRootResolver = displayRootResolver;
            _sectionBuilder = sectionBuilder;
            _resolverFactory = resolverFactory;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task<string> TranslateAsync(string contents, EventConfig eventDetails)
            => TranslateAsync(contents, eventDetails, null);

        /// <inheritdoc />
        public async Task<string> TranslateAsync(string contents, EventConfig eventDetails, IReadOnlyList<string> preferredRootFieldOrder)
        {
            if (eventDetails?.Display == null || eventDetails.Display.Count == 0)
            {
                _logger.LogWarning(
                    "Display: event {EventName} has no display configuration, so its stored contents cannot be shown. Add a Display array to the event configuration",
                    eventDetails?.EventName ?? "(unknown)");
                return JsonConvert.SerializeObject(new List<JsonItemModel>());
            }

            var displayRoot = _displayRootResolver.Resolve(contents, eventDetails);

            var fieldsCollector = new JsonWholeStructureFieldsCollector();
            var fields = fieldsCollector.GetStructure(displayRoot);

            var fieldsToDisplay = _sectionBuilder.Build(fields, eventDetails, preferredRootFieldOrder);

            _logger.LogDebug(
                "Display: event {EventName} has {DisplayCount} display rows and {RootFieldCount} payload root fields, showing {ShownCount}",
                eventDetails.EventName,
                eventDetails.Display.Count,
                fields?.Count ?? 0,
                fieldsToDisplay.Count);

            foreach (var field in fieldsToDisplay)
            {
                await TranslateFieldTreeAsync(field, eventDetails);
            }

            return JsonConvert.SerializeObject(fieldsToDisplay);
        }

        /// <summary>
        /// Translates a field's label, resolves its value, and repeats for every nested collection row and child
        /// object beneath it so nested blocks read the same as top level fields.
        /// </summary>
        /// <param name="field">Field to mutate in place.</param>
        /// <param name="eventDetails">Event configuration supplying the display rows.</param>
        private async Task TranslateFieldTreeAsync(JsonItemModel field, EventConfig eventDetails)
        {
            if (field == null)
            {
                return;
            }

            await TranslateSingleFieldAsync(field, eventDetails);

            foreach (var arrayField in field.ArrayItems ?? Enumerable.Empty<JsonArrayModel>())
            {
                foreach (var childItem in arrayField?.ChildItems ?? Enumerable.Empty<JsonItemModel>())
                {
                    await TranslateFieldTreeAsync(childItem, eventDetails);
                }
            }

            foreach (var childItem in field.ChildItems ?? Enumerable.Empty<JsonItemModel>())
            {
                await TranslateFieldTreeAsync(childItem, eventDetails);
            }
        }

        /// <summary>
        /// Translates a single field label and resolves its stored value to display text. Resolution is chosen in
        /// order: the display row's named resolver, list resolution for <c>List: 'Yes'</c>, then the organisation
        /// and organism defaults that apply by field name for configurations written before resolvers existed.
        /// </summary>
        /// <param name="field">Field to mutate in place.</param>
        /// <param name="eventDetails">Event configuration supplying the display rows.</param>
        private async Task TranslateSingleFieldAsync(JsonItemModel field, EventConfig eventDetails)
        {
            var fieldId = string.IsNullOrEmpty(field.Key) ? field.Label : field.Key;
            var display = eventDetails.Display.FirstOrDefault(f => f.Label.IsSameAs(fieldId));

            if (display == null)
            {
                return;
            }

            var resolverName = ResolverNameFor(display, fieldId);

            if (!string.IsNullOrEmpty(resolverName))
            {
                await ResolveContentsAsync(field, resolverName, fieldId, eventDetails);
            }

            field.Label = display.Translation;
        }

        /// <summary>
        /// Decides which resolver a display row should use.
        /// </summary>
        /// <param name="display">Display row for the field.</param>
        /// <param name="fieldId">Untranslated field id.</param>
        /// <returns>The resolver name, or null when the value should be shown as stored.</returns>
        private static string ResolverNameFor(DisplayConfig display, string fieldId)
        {
            if (!string.IsNullOrWhiteSpace(display.Resolver))
            {
                return display.Resolver;
            }

            if (display.List == "Yes")
            {
                return "list";
            }

            if (fieldId.IsSameAs("organisationid"))
            {
                return "organisation";
            }

            if (fieldId.IsSameAs("organismid"))
            {
                return "organism";
            }

            return null;
        }

        /// <summary>
        /// Replaces a field's stored value with resolved display text, leaving the stored value in place when the
        /// resolver is unregistered so a configuration mistake never blanks out data.
        /// </summary>
        /// <param name="field">Field to mutate in place.</param>
        /// <param name="resolverName">Name of the resolver to apply.</param>
        /// <param name="fieldId">Untranslated field id, used for logging.</param>
        /// <param name="eventDetails">Event configuration, used for logging.</param>
        private async Task ResolveContentsAsync(JsonItemModel field, string resolverName, string fieldId, EventConfig eventDetails)
        {
            if (string.IsNullOrWhiteSpace(field.Contents) || field.Contents == "null")
            {
                field.Contents = "";
                return;
            }

            var resolver = _resolverFactory.Get(resolverName);
            if (resolver == null)
            {
                return;
            }

            var storedValue = field.Contents;
            var resolved = await resolver.ResolveAsync(storedValue);

            if (string.IsNullOrEmpty(resolved) && HasResolvableId(storedValue))
            {
                _logger.LogWarning(
                    "Display: resolver '{ResolverName}' returned nothing for field {FieldId} value '{StoredValue}' on event {EventName}; the referenced record may have been deleted or renamed",
                    resolverName,
                    fieldId,
                    storedValue,
                    eventDetails.EventName);
            }

            field.Contents = resolved;
        }

        /// <summary>
        /// Returns whether a stored value contains at least one positive id, which is what makes an empty
        /// resolver result worth warning about rather than simply an unset field.
        /// </summary>
        /// <param name="storedValue">Stored value to inspect.</param>
        /// <returns>True when the value holds a positive id.</returns>
        private static bool HasResolvableId(string storedValue)
            => storedValue
                .Split(',')
                .Any(part => int.TryParse(part.Trim(), out var id) && id > 0);
    }
}
