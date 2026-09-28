using arc.app.Common;
using arc.app.Config.Events;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.WorkflowsConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Loads a single workflow for the workflow designer, together with the state, event and list
/// lookups the designer needs to show ids as readable text.
/// </summary>
/// <remarks>
/// The workflow document is returned exactly as stored. Only the supporting lookups are derived,
/// and those are never written back, so the designer cannot damage listitem or the event
/// configuration. Everything the designer matches on is an id: states are listitem ids and events
/// are matched by name, case-insensitively.
/// </remarks>
public class WorkflowDesignerHandler(
    IConfigRepository configRepository,
    IListRepository listRepository,
    IEventAdapter eventAdapter,
    ILogWriter logWriter) : IWorkflowDesignerHandler
{
    /// <summary>
    /// The ConfigTypeId workflow documents are stored under.
    /// </summary>
    internal const int WorkflowConfigTypeId = 18;

    /// <summary>
    /// Loads one workflow for the designer.
    /// </summary>
    /// <param name="workflowId">The configs identity and name of the workflow, as sent by the designer.</param>
    /// <returns>
    /// The workflow document and its supporting lookups. When the record cannot be found or read the
    /// model is returned with a null <see cref="WorkflowDesignerConfigModel.Workflow"/> and the reason
    /// is logged, so the designer shows an empty canvas rather than silently editing something else.
    /// </returns>
    public async Task<WorkflowDesignerConfigModel> GetWorkflowDesignerConfigurationAsync(IdAndNameModel workflowId)
    {
        var model = new WorkflowDesignerConfigModel { ConfigName = workflowId?.Name };

        if (!int.TryParse(workflowId?.Id, out var configId) || configId <= 0)
        {
            logWriter.LogError(
                $"Workflow designer was asked for workflow '{workflowId?.Name}' with id '{workflowId?.Id}', which is not a configs identity. Nothing was loaded.",
                nameof(WorkflowDesignerHandler), nameof(GetWorkflowDesignerConfigurationAsync));
            return model;
        }

        logWriter.LogInfo(
            $"Loading workflow designer configuration for configs id {configId} ('{workflowId.Name}').",
            nameof(WorkflowDesignerHandler), nameof(GetWorkflowDesignerConfigurationAsync));

        var configRecord = await LoadWorkflowRecordAsync(configId);
        if (configRecord == null)
        {
            return model;
        }

        model.ConfigId = configRecord.Id;
        model.ConfigName = configRecord.ConfigName;

        var workflow = DeserialiseWorkflow(configRecord.Contents, configRecord.ConfigName, configRecord.Id);
        if (workflow == null)
        {
            return model;
        }

        model.Workflow = JObject.Parse(configRecord.Contents);
        model.SupportingConfig.StateConfig = await LoadStatesAsync(workflow, configRecord.ConfigName);
        model.SupportingConfig.EventConfig = await LoadEventsAsync(workflow);
        model.SupportingConfig.ListConfig = await LoadListsAsync(model.SupportingConfig.EventConfig);

        logWriter.LogInfo(
            $"Loaded workflow '{configRecord.ConfigName}' (id {configRecord.Id}): {workflow.Steps?.Count ?? 0} step(s), " +
            $"{model.SupportingConfig.StateConfig.Count} state(s) from StatesList '{workflow.StatesList}', " +
            $"{model.SupportingConfig.EventConfig.Count} event(s), {model.SupportingConfig.ListConfig.Count} list(s).",
            nameof(WorkflowDesignerHandler), nameof(GetWorkflowDesignerConfigurationAsync));

        return model;
    }

    /// <summary>
    /// Reads the configs row for a workflow and confirms it really is a workflow.
    /// </summary>
    /// <param name="configId">The configs identity the designer asked for.</param>
    /// <returns>The configuration record, or null when it is missing, empty or of the wrong type.</returns>
    private async Task<arc.data.model.Configuration.ConfigsDataModel> LoadWorkflowRecordAsync(int configId)
    {
        var filters = new QueryFilterConfig();
        filters.AddString("id", configId.ToString());

        var configRecord = await configRepository.SingleConfigByIdAsync(filters);

        if (configRecord == null || configRecord.Id == 0)
        {
            logWriter.LogError(
                $"No configs row found for workflow id {configId}.",
                nameof(WorkflowDesignerHandler), nameof(LoadWorkflowRecordAsync));
            return null;
        }

        if (configRecord.ConfigTypeId != WorkflowConfigTypeId)
        {
            logWriter.LogError(
                $"Configs row {configId} ('{configRecord.ConfigName}') is ConfigType {configRecord.ConfigTypeId}, not the workflow type {WorkflowConfigTypeId}. Refusing to load it as a workflow.",
                nameof(WorkflowDesignerHandler), nameof(LoadWorkflowRecordAsync));
            return null;
        }

        if (string.IsNullOrWhiteSpace(configRecord.Contents) || configRecord.Contents == "{}")
        {
            logWriter.LogError(
                $"Workflow '{configRecord.ConfigName}' (id {configId}) has empty contents.",
                nameof(WorkflowDesignerHandler), nameof(LoadWorkflowRecordAsync));
            return null;
        }

        return configRecord;
    }

    /// <summary>
    /// Reads the stored document as a workflow so the parts the designer needs can be inspected.
    /// The typed result is used only to derive the lookups; the document sent to the designer is the
    /// stored JSON untouched.
    /// </summary>
    /// <param name="contents">The stored configs contents.</param>
    /// <param name="configName">The configuration name, for logging.</param>
    /// <param name="configId">The configs identity, for logging.</param>
    /// <returns>The workflow, or null when the document could not be read.</returns>
    private WorkflowConfig DeserialiseWorkflow(string contents, string configName, int configId)
    {
        try
        {
            return JsonConvert.DeserializeObject<WorkflowConfig>(contents);
        }
        catch (JsonException ex)
        {
            logWriter.LogError(
                $"Workflow '{configName}' (id {configId}) could not be read as a workflow document: {ex.Message}",
                nameof(WorkflowDesignerHandler), nameof(DeserialiseWorkflow));
            return null;
        }
    }

    /// <summary>
    /// Resolves the workflow's states from the list item its StatesList points at.
    /// </summary>
    /// <remarks>
    /// StatesList holds a list item id, not a list id: the specimen states live in a hierarchical list
    /// and are the children of that item. Reading them from the database rather than a hardcoded table
    /// is what makes the designer show translated state names and stay correct when a laboratory adds a
    /// state or points a workflow at a different set of states.
    /// </remarks>
    /// <param name="workflow">The workflow being loaded.</param>
    /// <param name="configName">The configuration name, for logging.</param>
    /// <returns>The states, keyed by listitem id.</returns>
    private async Task<List<WorkflowOptionModel>> LoadStatesAsync(WorkflowConfig workflow, string configName)
    {
        if (!int.TryParse(workflow.StatesList?.Trim(), out var statesListItemId) || statesListItemId <= 0)
        {
            logWriter.LogError(
                $"Workflow '{configName}' has StatesList '{workflow.StatesList}', which is not a list item id, so the designer has no state names to show.",
                nameof(WorkflowDesignerHandler), nameof(LoadStatesAsync));
            return [];
        }

        var options = await listRepository.GetListValuesByParentIdAsync(statesListItemId);

        var states = (options ?? [])
            .Select(option => new WorkflowOptionModel { Key = option.Key, Value = option.Text })
            .ToList();

        if (states.Count == 0)
        {
            logWriter.LogError(
                $"Workflow '{configName}' has StatesList '{workflow.StatesList}', which has no child list items. The designer will render an empty canvas.",
                nameof(WorkflowDesignerHandler), nameof(LoadStatesAsync));
        }

        return states;
    }

    /// <summary>
    /// Builds the event lookup from the event configurations the workflow's steps and entry
    /// conditions actually reference, so conditions are offered against real payload field ids.
    /// </summary>
    /// <param name="workflow">The workflow being loaded.</param>
    /// <returns>One entry per distinct event named by the workflow.</returns>
    private async Task<List<WorkflowEventConfigModel>> LoadEventsAsync(WorkflowConfig workflow)
    {
        var events = new List<WorkflowEventConfigModel>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var eventName in CollectEventNames(workflow))
        {
            if (!seen.Add(eventName))
            {
                continue;
            }

            try
            {
                var eventConfig = await eventAdapter.GetEventAsync(eventName);
                if (eventConfig == null)
                {
                    logWriter.LogError(
                        $"Workflow references event '{eventName}', which has no event configuration. Conditions cannot be built against it.",
                        nameof(WorkflowDesignerHandler), nameof(LoadEventsAsync));
                    continue;
                }

                events.Add(new WorkflowEventConfigModel
                {
                    EventName = eventName,
                    EventDescription = eventConfig.Description,
                    Fields = BuildFields(eventConfig)
                });
            }
            catch (Exception ex)
            {
                logWriter.LogError(
                    $"Could not load the event configuration for '{eventName}': {ex.Message}",
                    nameof(WorkflowDesignerHandler), nameof(LoadEventsAsync));
            }
        }

        return events;
    }

    /// <summary>
    /// Collects every event name the workflow mentions, from its steps and its entry conditions.
    /// </summary>
    /// <param name="workflow">The workflow being loaded.</param>
    /// <returns>The event names, in the order they appear.</returns>
    private static IEnumerable<string> CollectEventNames(WorkflowConfig workflow)
    {
        foreach (var step in workflow.Steps ?? [])
        {
            if (!string.IsNullOrWhiteSpace(step?.Event))
            {
                yield return step.Event.Trim();
            }
        }

        foreach (var condition in workflow.EntryConditions ?? [])
        {
            foreach (var name in (condition?.Events ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    yield return name.Trim();
                }
            }
        }
    }

    /// <summary>
    /// Projects an event's display rows into the fields the designer offers when building a condition.
    /// </summary>
    /// <remarks>
    /// A display row's <c>Label</c> is the field id as it appears in the stored payload and its
    /// <c>Translation</c> is the @Tag@ language token shown to the user, so the id is what a condition
    /// is written against and the token is only ever displayed. A field is linked to an option list
    /// when the event declares a list whose name matches the field id, with or without a trailing
    /// <c>Id</c>; that is the only association the event configuration actually records.
    /// </remarks>
    /// <param name="eventConfig">The event configuration to project.</param>
    /// <returns>The fields available for conditions on this event.</returns>
    private static List<WorkflowEventFieldModel> BuildFields(EventConfig eventConfig)
    {
        var listNames = (eventConfig.Lists ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(name => name.Trim())
            .Where(name => name.Length > 0)
            .ToList();

        return
        [
            .. (eventConfig.Display ?? [])
                .Where(display => !string.IsNullOrWhiteSpace(display?.Label) && !display.Grid)
                .Select(display => new WorkflowEventFieldModel
                {
                    Id = display.Label,
                    Label = string.IsNullOrWhiteSpace(display.Translation) ? display.Label : display.Translation,
                    Type = ResolveFieldType(display),
                    OptionsName = ResolveOptionsName(display, listNames)
                })
        ];
    }

    /// <summary>
    /// Decides how the designer should let a condition value be entered for a field.
    /// </summary>
    /// <param name="display">The event display row for the field.</param>
    /// <returns>"list", "date" or "text".</returns>
    private static string ResolveFieldType(DisplayConfig display)
    {
        if (string.Equals(display.List, "Yes", StringComparison.OrdinalIgnoreCase))
        {
            return "list";
        }

        return display.Date ? "date" : "text";
    }

    /// <summary>
    /// Finds the option list that supplies a field's values, if the event declares one.
    /// </summary>
    /// <param name="display">The event display row for the field.</param>
    /// <param name="listNames">The list names declared by the event.</param>
    /// <returns>The list name, or null when the field has no list.</returns>
    private static string ResolveOptionsName(DisplayConfig display, List<string> listNames)
    {
        if (!string.Equals(display.List, "Yes", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var fieldId = display.Label.Trim();
        var withoutIdSuffix = fieldId.EndsWith("Id", StringComparison.OrdinalIgnoreCase) && fieldId.Length > 2
            ? fieldId[..^2]
            : fieldId;

        return listNames.FirstOrDefault(name =>
            string.Equals(name, fieldId, StringComparison.OrdinalIgnoreCase)
            || string.Equals(name, withoutIdSuffix, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Loads the contents of every option list referenced by an event field.
    /// </summary>
    /// <param name="events">The event lookup already built for the workflow.</param>
    /// <returns>One entry per distinct list, keyed by listitem id.</returns>
    private async Task<List<WorkflowListConfigModel>> LoadListsAsync(List<WorkflowEventConfigModel> events)
    {
        var lists = new List<WorkflowListConfigModel>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var optionsNames = events
            .SelectMany(item => item.Fields)
            .Select(field => field.OptionsName)
            .Where(name => !string.IsNullOrWhiteSpace(name));

        foreach (var optionsName in optionsNames)
        {
            if (!seen.Add(optionsName))
            {
                continue;
            }

            try
            {
                var options = await listRepository.GetListValuesAsync(optionsName, true);

                lists.Add(new WorkflowListConfigModel
                {
                    OptionsName = optionsName,
                    Contents = [.. (options ?? []).Select(option => new WorkflowOptionModel { Key = option.Key, Value = option.Text })]
                });
            }
            catch (Exception ex)
            {
                logWriter.LogError(
                    $"Could not load the option list '{optionsName}' for the workflow designer: {ex.Message}",
                    nameof(WorkflowDesignerHandler), nameof(LoadListsAsync));
            }
        }

        return lists;
    }
}
