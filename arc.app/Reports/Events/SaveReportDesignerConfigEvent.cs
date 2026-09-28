using arc.app.Common;
using arc.app.Config.Reports;
using arc.app.SystemConfig;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;

namespace arc.app.Reports.Events
{
    /// <summary>
    /// Event handler for saving report designer configuration changes.
    /// Processes the complete report configuration including changed sections and custom formats.
    /// </summary>
    internal class SaveReportDesignerConfigEvent : IRun, IProvideSaveResult<ReportDesignerSaveResultModel>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the SaveReportDesignerConfigEvent class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public SaveReportDesignerConfigEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the outcome of the most recent save, so the controller can return it to the designer.
        /// </summary>
        public ReportDesignerSaveResultModel SaveResult { get; private set; }

        /// <summary>
        /// Executes the save report designer configuration event.
        /// </summary>
        /// <param name="dataToSave">The JSON string containing the report configuration data.</param>
        /// <param name="id">The identifier for the operation.</param>
        /// <param name="command">The event model containing command information.</param>
        /// <param name="eventData">Optional event configuration data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of configuration records written.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            var reportConfigRepository = _serviceProvider.GetService<IReportConfigRepository>();
            var config = JsonConvert.DeserializeObject<SaveReportDesignerConfigModel>(dataToSave);

            if (config == null)
            {
                logWriter?.LogError(
                    $"Report designer payload for '{id}' deserialised to null. Nothing was saved.",
                    nameof(SaveReportDesignerConfigEvent), nameof(RunAsync));
                SaveResult = new ReportDesignerSaveResultModel { ReportName = id };
                return 0;
            }

            LogBindingLosses(dataToSave, config, logWriter);
            await EnsureImmutableReportNameAsync(config, id, logWriter);

            logWriter?.LogInfo(
                $"Report designer payload for '{config.Name}' bound to {config.ChangedCustomFormats?.Count ?? 0} format(s), {config.ChangedSectionDefinitions?.Count ?? 0} section(s), {config.DeletedFormats?.Count ?? 0} format deletion(s), {config.DeletedSections?.Count ?? 0} section deletion(s).",
                nameof(SaveReportDesignerConfigEvent), nameof(RunAsync));

            SaveResult = await reportConfigRepository.SaveReportDesignerConfigAsync(config);

            return (SaveResult?.Formats?.Count ?? 0) + (SaveResult?.Sections?.Count ?? 0);
        }

        /// <summary>
        /// Compares the raw payload with the bound model and logs an error when a change list arrived
        /// populated but bound empty. This is what makes a silent binding failure visible in an installed
        /// system, where the symptom is simply that a change is never written.
        /// </summary>
        /// <param name="dataToSave">The raw JSON payload received from the controller.</param>
        /// <param name="config">The bound configuration model.</param>
        /// <param name="logWriter">The log writer.</param>
        private static void LogBindingLosses(string dataToSave, SaveReportDesignerConfigModel config, ILogWriter logWriter)
        {
            if (logWriter == null)
            {
                return;
            }

            try
            {
                var raw = JObject.Parse(dataToSave);
                CheckList(raw, "ChangedCustomFormats", config.ChangedCustomFormats?.Count ?? 0, logWriter);
                CheckList(raw, "ChangedSectionDefinitions", config.ChangedSectionDefinitions?.Count ?? 0, logWriter);
            }
            catch (JsonException ex)
            {
                logWriter.LogError(
                    $"Could not re-read the report designer payload to check for binding losses: {ex.Message}",
                    nameof(SaveReportDesignerConfigEvent), nameof(LogBindingLosses));
            }
        }

        private static void CheckList(JObject raw, string propertyName, int boundCount, ILogWriter logWriter)
        {
            var rawCount = (raw[propertyName] as JArray)?.Count ?? 0;
            if (rawCount > 0 && boundCount == 0)
            {
                logWriter.LogError(
                    $"'{propertyName}' arrived with {rawCount} item(s) but bound to none. The payload shape does not match SaveReportDesignerConfigModel.",
                    nameof(SaveReportDesignerConfigEvent), nameof(LogBindingLosses));
            }
        }

        /// <summary>
        /// Ensures the report designer save payload uses the stored report name. Report names are
        /// immutable identifiers assigned at clone time; a stale client or manual API call must not
        /// rename the config record.
        /// </summary>
        /// <param name="config">The bound save payload.</param>
        /// <param name="routeName">The report name supplied by the controller (same as the loaded config).</param>
        /// <param name="logWriter">The log writer.</param>
        private async Task EnsureImmutableReportNameAsync(
            SaveReportDesignerConfigModel config,
            string routeName,
            ILogWriter logWriter)
        {
            if (config == null)
            {
                return;
            }

            var lookupName = routeName.NormalisedConfigName();
            if (lookupName.Length == 0)
            {
                return;
            }

            if (!config.Name.IsSameConfigName(lookupName))
            {
                logWriter?.LogWarning(
                    $"Report designer save payload Name '{config.Name}' does not match route name '{routeName}'. Normalising to the route name because report names are immutable.",
                    nameof(SaveReportDesignerConfigEvent), nameof(EnsureImmutableReportNameAsync));
                config.Name = lookupName;
            }

            var reportAdapter = _serviceProvider.GetService<IReportAdapter>();
            if (reportAdapter == null)
            {
                return;
            }

            try
            {
                var existing = await reportAdapter.GetReportAsync(lookupName);
                if (existing != null && !config.Name.IsSameConfigName(existing.Name))
                {
                    logWriter?.LogWarning(
                        $"Report designer save attempted to use Name '{config.Name}' but the stored report is '{existing.Name}'. Using the stored name.",
                        nameof(SaveReportDesignerConfigEvent), nameof(EnsureImmutableReportNameAsync));
                    config.Name = existing.Name.NormalisedConfigName();
                }
            }
            catch (Exception ex)
            {
                logWriter?.LogWarning(
                    $"Could not verify stored report name for '{lookupName}' before save: {ex.Message}",
                    nameof(SaveReportDesignerConfigEvent), nameof(EnsureImmutableReportNameAsync));
            }
        }
    }
}
