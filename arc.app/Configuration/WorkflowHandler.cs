using arc.app.Common;
using arc.app.Config;
using arc.app.Config.Workflows;
using arc.app.Quality;
using arc.app.Specimen;
using arc.app.SystemConfig;
using arc.app.Tests;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.Specimen;
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
/// Handles workflow-related operations, including retrieving workflows and managing state transitions.
/// Implements IWorkflowHandler.
/// </summary>
public class WorkflowHandler : IWorkflowHandler
{
    private readonly IWorkflowAdapter _workflowAdapter;
    private readonly IAllBaseViewConfigFactory _allBaseViewConfigFactory;
    private readonly IStateHandler _stateHandler;
    private readonly ICultureRepository _cultureRepository;
    private readonly ITestRepository _testRepository;
    private readonly IConfigRepository _configRepository;
    private readonly IWorkflowFinder _workflowFinder;
    private readonly IQualityRepository _qualityRepository;
    private readonly IMessageQueue _messageQueue;
    private readonly ILogWriter _logWriter;

    private WorkflowConfig _workflowConfig;

    /// <summary>
    /// Initializes a new instance of the WorkflowHandler class with dependencies for handling workflows.
    /// </summary>
    public WorkflowHandler(
        IWorkflowAdapter workflowAdapter,
        IAllBaseViewConfigFactory allBaseViewConfigFactory,
        IStateHandler stateHandler,
        ICultureRepository cultureRepository,
        ITestRepository testRepository,
        IConfigRepository configRepository,
        IQualityRepository qualityRepository,
        IWorkflowFinder workflowFinder,
        IMessageQueue messageQueue,
        ILogWriter logWriter)
    {
        _workflowAdapter = workflowAdapter;
        _allBaseViewConfigFactory = allBaseViewConfigFactory;
        _stateHandler = stateHandler;
        _cultureRepository = cultureRepository;
        _testRepository = testRepository;
        _configRepository = configRepository;
        _workflowFinder = workflowFinder;
        _qualityRepository = qualityRepository;
        _messageQueue = messageQueue;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Asynchronously retrieves a list of all available workflow configurations.
    /// </summary>
    /// <returns>A list of WorkflowConfig objects.</returns>
    public async Task<List<WorkflowConfig>> GetWorkflowListAsync()
    {
        var parameters = new QueryFilterConfig().AddString("ConfigTypeId", "18");
        var workflowList = await _configRepository.GetConfigListAsync(parameters);

        return workflowList.Select(workflow =>
        {
            var newItem = JsonConvert.DeserializeObject<WorkflowConfig>(workflow.Contents);
            newItem.Id = workflow.Id;
            return newItem;
        }).ToList();
    }

    /// <summary>
    /// Asynchronously retrieves a single workflow configuration by name.
    /// </summary>
    /// <param name="workflowName">The name of the workflow.</param>
    /// <returns>The WorkflowConfig object.</returns>
    public async Task<WorkflowConfig> GetSingleWorkflowAsync(string workflowName)
    {
        return await _workflowAdapter.GetWorkflowAsync(workflowName);
    }

    /// <summary>
    /// Determines the next workflow state based on an event trigger.
    /// </summary>
    /// <param name="message">The input message containing relevant identifiers.</param>
    /// <param name="model">The event model containing the current state.</param>
    /// <param name="eventData">Additional event configuration data.</param>
    /// <param name="token">Authentication token for laboratory context.</param>
    /// <returns>An updated event model with the new workflow state.</returns>
    public async Task<EventModel> GetNextStateAsync(string message, EventModel model, EventConfig eventData, TokenInfoModel token)
    {
        if (eventData != null && !eventData.UsesSpecimenWorkflowResolution())
        {
            return model;
        }

        var view = _allBaseViewConfigFactory.Get(model.View);
        var workflowName = view?.Workflow;
        if (string.IsNullOrEmpty(workflowName))
        {
            // Specimen events (e.g. Add Specimen from Patients list) may post View=patients, which
            // intentionally has no view-level workflow. Resolve via SpecimenDefault on save.
            workflowName = "SpecimenDefault";
            _logWriter.LogInfo(
                $"View {model.View} has no workflow binding; using {workflowName} for event {model.Event}",
                "WorkflowHandler",
                "GetNextStateAsync");
        }

        var idModel = JsonConvert.DeserializeObject<IdAndSpecimenTypeIdModel>(message, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        var workflow = await _workflowAdapter.GetWorkflowAsync(workflowName);
        if (workflowName.IsSameAs("SpecimenDefault"))
        {
            _ = int.TryParse(idModel?.Id, out int parsedId);

            var specimenPatientData = await _messageQueue.GetSpecimenPatientDetailsForWorkflowAsync(eventData, parsedId);
            // When no specimen context (SpecimenId=0), pass 0 to avoid treating LaboratoryId/other IDs as specimen IDs
            var specimenIdForWorkflow = specimenPatientData.SpecimenId > 0 ? specimenPatientData.SpecimenId : 0;
            workflow = await _workflowFinder.GetCurrentWorkflowAsync(specimenIdForWorkflow, idModel.SpecimenTypeId, idModel.LaboratoryId, token);
            _logWriter.LogInfo(
                $"Workflow resolve: event={model.Event}, view={model.View}, parsedId={parsedId}, specimenIdForWorkflow={specimenIdForWorkflow}, workflow={workflow?.Name}",
                "WorkflowHandler",
                "GetNextStateAsync");
        }

        _workflowConfig = workflow;

        if (workflow != null && workflow.CheckIfEventInWorkflow(model.Event))
        {
            model.StateId = "";

            // Retrieve the current workflow state based on event type and related objects.
            if ((eventData.EventType.IsNotSameAs("adddata") && eventData.EventType.IsNotSameAs("specialadddata")) || eventData.TableName.IsNotSameAs(workflow.Table))
            {
                if (!string.IsNullOrEmpty(idModel.Id))
                {
                    string id = idModel.Id;
                    // If we're updating a non-primary object (e.g. a culture), we need to get the Id of the primary object (specimen).
                    if (eventData.EventType.ToLower() != "adddata" && eventData.EventName.ToLower() != "addculture" && model.Event != "TestSelection" && model.Event != "CultureTestSelection")
                    {
                        if (eventData.TableName.ToLower() == "culture" || eventData.TableName.ToLower() == "ast" || eventData.TableName.ToLower() == "culturetests")
                        {
                            if (eventData.TableName.ToLower() == "culturetests")
                            {
                                var testDetails = await _testRepository.GetCultureTestAsync(int.Parse(id));
                                id = testDetails.CultureId.ToString();
                            }
                            // Get specimen Id from culture record.
                            var specimenDetails = await _cultureRepository.GetSingleAsync(int.Parse(id));
                            id = specimenDetails.SpecimenId.ToString();
                            model.StateTargetId = id;
                        }

                        else if (eventData.TableName.ToLower() == "tests")
                        {
                            // Get specimen Id from test record.
                            var specimenDetails = await _testRepository.GetTestAsync(int.Parse(id));
                            id = specimenDetails.SpecimenId.ToString();
                            model.StateTargetId = id;
                        }
                    }
                    if (model.Event == "CultureTestSelection")
                    {
                        // Get specimen Id from culture record.
                        var specimenDetails = await _cultureRepository.GetSingleAsync(int.Parse(id));
                        id = specimenDetails.SpecimenId.ToString();
                    }
                    if (model.Event == "editiqcresult")
                    {
                        var jsonObject = JObject.Parse(message);
                        id = (string)jsonObject["Crafted"][0]["Contents"][0]["value"];
                    }
                    if (model.Event == "deleteiqcresult")
                    {
                        var iqcTestIdInt = await _qualityRepository.GetIqcTestIdFromIqcResultIdAsync(int.Parse(id));
                        id = iqcTestIdInt.ToString();
                    }
                    if (model.Event != "approvereportevent" && model.Event != "unapprovereportevent")
                    {
                        model.StateId = await _stateHandler.GetCurrentStateFromDatabaseAsync(id, workflow.Table, workflow.Field);
                    }
                }
            }

            var workflowMessage = NormalizeCultureWorkflowMessage(message, eventData);
            model.NewStateId = workflow.GetNextState(model.StateId, model.Event, workflowMessage);
            model.Table = workflow.Table;
            model.Field = workflow.Field;

            if (eventData.TableName.IsSameAs("culture")
                && !string.IsNullOrEmpty(model.StateId)
                && !string.IsNullOrEmpty(model.NewStateId)
                && !model.NewStateId.IsSameAs(model.StateId))
            {
                _logWriter.LogInfo(
                    $"Specimen workflow state transition: event={model.Event}, cultureId={idModel?.Id}, specimenId={model.StateTargetId}, previousStateId={model.StateId}, newStateId={model.NewStateId}",
                    nameof(WorkflowHandler),
                    nameof(GetNextStateAsync));
            }
        }

        return model;
    }

    /// <summary>
    /// Maps <c>GrowthId</c> from culture event payloads to <c>Quantity</c> when growth is present,
    /// so legacy <c>SpecimenDefault</c> workflow steps that key on <c>Quantity</c> evaluate the
    /// edited growth value rather than a stale <c>Quantity</c> loaded from <c>SpecimenQuantityId</c>.
    /// </summary>
    private string NormalizeCultureWorkflowMessage(string message, EventConfig eventData)
    {
        if (eventData?.TableName == null || !eventData.TableName.IsSameAs("culture") || string.IsNullOrWhiteSpace(message))
        {
            return message;
        }

        try
        {
            var jsonObject = JObject.Parse(message);
            var growthToken = jsonObject.GetValue("GrowthId", StringComparison.OrdinalIgnoreCase)
                ?? jsonObject.GetValue("growthid", StringComparison.OrdinalIgnoreCase);
            if (growthToken == null || growthToken.Type == JTokenType.Null)
            {
                return message;
            }

            var existingQuantity = jsonObject.GetValue("Quantity", StringComparison.OrdinalIgnoreCase)?.ToString();
            var growthValue = growthToken.ToString();
            if (existingQuantity.IsSameAs(growthValue))
            {
                return message;
            }

            jsonObject["Quantity"] = growthValue;
            _logWriter.LogInfo(
                $"Culture workflow: mapped GrowthId={growthValue} to Quantity (was {(string.IsNullOrEmpty(existingQuantity) ? "absent" : existingQuantity)}) for event {eventData.EventName}",
                nameof(WorkflowHandler),
                nameof(NormalizeCultureWorkflowMessage));
            return jsonObject.ToString();
        }
        catch (JsonReaderException)
        {
            return message;
        }
    }

    /// <summary>
    /// Retrieves the action associated with the current workflow step.
    /// </summary>
    /// <param name="model">The event model with workflow data.</param>
    /// <param name="message">Additional context for the action lookup.</param>
    /// <returns>The action configuration matching the current workflow state.</returns>
    public List<ActionConditionConfig> GetActions(EventModel model, string message)
    {
        if (_workflowConfig != null)
        {
            var steps = _workflowConfig.Steps.Where(s => s.Event.Equals(model.Event, System.StringComparison.CurrentCultureIgnoreCase));

            if (steps.Any())
            {
                var actionsToCarryOut = new List<ActionConditionConfig>();
                foreach(var step in steps)
                {
                    var actionToCarryOut = step.Actions.Options.Where(o => o.NewState == model.NewStateId);
                    if (actionToCarryOut.Any())
                    {
                        foreach(var action in actionToCarryOut)
                        {
                            if(action.IsConditionMatched(null, message))
                            {
                                actionsToCarryOut.Add(action);
                            }
                        }
                    }
                }
                return actionsToCarryOut;
            }
        }

        return [];
    }
}