using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Event handler responsible for upserting the JSON or XML structural mapper for an export profile.
    /// Validates the payload and delegates persistence to <see cref="IExportProfileMappingHandler"/>.
    /// </summary>
    internal class SaveExportProfileMappingEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveExportProfileMappingEvent"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve handlers and loggers.</param>
        public SaveExportProfileMappingEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var handler = _serviceProvider.GetService<IExportProfileMappingHandler>();
            var logWriter = _serviceProvider.GetService<ILogWriter>();

            logWriter?.LogInfo("Save export profile mapping event invoked", nameof(SaveExportProfileMappingEvent), nameof(RunAsync));

            if (string.IsNullOrWhiteSpace(dataToSave))
            {
                logWriter?.LogInfo("WARN: Save export profile mapping called with empty payload", nameof(SaveExportProfileMappingEvent), nameof(RunAsync));
                throw new ArgumentException("Mapping data is required.", nameof(dataToSave));
            }

            var request = ExtractRequest(dataToSave, Id, logWriter);
            if (request == null || request.ExportProfileId <= 0)
            {
                logWriter?.LogInfo("WARN: Save export profile mapping payload missing ExportProfileId", nameof(SaveExportProfileMappingEvent), nameof(RunAsync));
                throw new ArgumentException("Mapping data must include an ExportProfileId.", nameof(dataToSave));
            }

            var savedId = await handler.SaveAsync(request);
            logWriter?.LogInfo($"Saved export profile mapping (id={savedId}, profileId={request.ExportProfileId})", nameof(SaveExportProfileMappingEvent), nameof(RunAsync));
            return savedId;
        }

        /// <summary>
        /// Extracts the <see cref="SaveExportProfileMappingRequest"/> from the raw save payload.
        /// Crafted pages wrap the actual payload inside a <c>Crafted[].Contents[]</c> structure;
        /// this helper unwraps it and falls back to a flat payload for direct API callers.
        /// </summary>
        /// <param name="dataToSave">The raw JSON payload posted by the form save pipeline.</param>
        /// <param name="id">The form identifier (used as a fallback for ExportProfileId).</param>
        /// <param name="logWriter">Optional logger used for diagnostic information.</param>
        /// <returns>The deserialized save request, or null when the payload is unrecognised.</returns>
        private static SaveExportProfileMappingRequest ExtractRequest(string dataToSave, string id, ILogWriter logWriter)
        {
            try
            {
                var crafted = JsonConvert.DeserializeObject<CraftedWithIdForEventModel<ExportProfileMappingCraftedModel>>(dataToSave);
                var inner = crafted?.Crafted?.Count > 0 ? crafted.Crafted[0]?.Contents : null;
                if (inner != null && inner.Count > 0 && inner[0]?.Value != null)
                {
                    var req = inner[0].Value;
                    if (req.ExportProfileId <= 0 && int.TryParse(crafted.Id ?? id, out var parsedId))
                    {
                        req.ExportProfileId = parsedId;
                    }
                    return req;
                }
            }
            catch (JsonException ex)
            {
                logWriter?.LogInfo($"WARN: Crafted-shaped deserialization failed, falling back to flat payload: {ex.Message}", nameof(SaveExportProfileMappingEvent), nameof(ExtractRequest));
            }

            try
            {
                var flat = JsonConvert.DeserializeObject<SaveExportProfileMappingRequest>(dataToSave);
                if (flat != null && flat.ExportProfileId <= 0 && int.TryParse(id, out var parsedId))
                {
                    flat.ExportProfileId = parsedId;
                }
                return flat;
            }
            catch (JsonException ex)
            {
                logWriter?.LogInfo($"WARN: Flat-payload deserialization failed: {ex.Message}", nameof(SaveExportProfileMappingEvent), nameof(ExtractRequest));
                return null;
            }
        }
    }
}
