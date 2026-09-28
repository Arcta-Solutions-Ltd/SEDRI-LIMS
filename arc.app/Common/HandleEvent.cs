using arc.app.Common;
using arc.app.Config.Events;
using arc.app.Config.Mapper;
using arc.app.Configuration;
using arc.app.Security;
using arc.common;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Transactions;
using arc.common.Models;
using arc.common.Models.Specimen;
using arc.app.Alert;
using System.Linq;
using Newtonsoft.Json.Linq;
using arc.common.ExtensionMethods;
using arc.domain.Configuration.EventsConfig;

namespace arc.app
{
    /// <summary>
    /// Handles events and processes them according to the specified logic.
    /// </summary>
    public class HandleEvent : IHandleEvent
    {
        private string _validationMessage = "";
        private readonly IGenericRepository _genericRepository;
        private readonly IMessageQueue _messageQueue;
        private readonly IEventAdapter _eventAdapter;
        private readonly IMapperAdapter _mapperAdapter;
        private readonly IProcessMapping _processMapping;
        private readonly ISpecialEventFactory _specialEventFactory;
        private readonly IWorkflowHandler _workflowHandler;
        private readonly IRegisterHandler _registerHandler;
        private readonly IDataRuleValidator _dataRuleValidator;
        private readonly ISpecialValidationFactory _specialValidationFactory;
        private readonly IActionHandler _actionHandler;
        private readonly IAlertHandler _alertHandler;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleEvent"/> class.
        /// </summary>
        /// <param name="genericRepository">The generic repository.</param>
        /// <param name="messageQueue">The message queue.</param>
        /// <param name="eventAdapter">The event adapter.</param>
        /// <param name="mapperAdapter">The mapper adapter.</param>
        /// <param name="processMapping">The process mapping service.</param>
        /// <param name="specialEventFactory">The special event factory.</param>
        /// <param name="workflowHandler">The workflow handler.</param>
        /// <param name="registerHandler">The register handler.</param>
        /// <param name="dataRuleValidator">The data rule validator.</param>
        /// <param name="specialValidationFactory">The special validation factory.</param>
        /// <param name="actionHandler">The action handler.</param>
        /// <param name="alertHandler">The alert handler.</param>
        /// <param name="logWriter">The log writer.</param>
        public HandleEvent (IGenericRepository genericRepository, IMessageQueue messageQueue, IEventAdapter eventAdapter, IMapperAdapter mapperAdapter, IProcessMapping processMapping,
                            ISpecialEventFactory specialEventFactory, IWorkflowHandler workflowHandler, IRegisterHandler registerHandler,
                            IDataRuleValidator dataRuleValidator, ISpecialValidationFactory specialValidationFactory, IActionHandler actionHandler, IAlertHandler alertHandler,
                            ILogWriter logWriter)
        {
            _genericRepository = genericRepository;
            _messageQueue = messageQueue;
            _eventAdapter = eventAdapter;
            _mapperAdapter = mapperAdapter;
            _processMapping = processMapping;
            _workflowHandler = workflowHandler;
            _specialEventFactory = specialEventFactory;
            _registerHandler = registerHandler;
            _dataRuleValidator = dataRuleValidator;
            _specialValidationFactory = specialValidationFactory;
            _actionHandler = actionHandler;
            _alertHandler = alertHandler;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Asynchronously handles an event based on the provided message and token information.
        /// </summary>
        /// <param name="message">The event message as a JSON string.</param>
        /// <param name="token">The token information model.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID model.</returns>
        public async Task<IdModel> HandleAsync(string message, TokenInfoModel token)
        {
            var queueId = 0;
            EventConfig eventData = null;
            try
            {
                var command = JsonConvert.DeserializeObject<EventModel>(message);

                _logWriter.LogInfo($"Carrying out event : {command.Event}", "HandleEvent", "Handle");

                // special handling of user events

                var eventName = command.Event.ToLower();
                if (eventName == "adduser" || eventName == "edituser" || eventName == "changepassword" || eventName == "mypassword")
                {
                    switch (eventName)
                    {
                        case "adduser":
                            await _registerHandler.UpdateUser(message, false);
                            break;
                        case "edituser":
                            await _registerHandler.UpdateUser(message, true);
                            break;
                        case "changepassword":
                            await _registerHandler.ChangePassword(message);
                            break;
                        case "mypassword":
                            await _registerHandler.ChangeMyPassword(message, token);
                            break;
                    }
                    _validationMessage = _registerHandler.GetValidationMessage();
                    return new IdModel();
                }

                // Get config data from event

                _logWriter.LogInfo("Get event configuration data", "HandleEvent", "Handle");
                eventData = await _eventAdapter.GetEventAsync(command.Event);

                // Validate the data

                _logWriter.LogInfo("Validate data for event", "HandleEvent", "Handle");

                _validationMessage = eventData.ValidateMessage(message);
                if (string.IsNullOrEmpty(_validationMessage)) { _validationMessage = _specialValidationFactory.ValidateMessage(eventData.EventName, message); }
                if (string.IsNullOrEmpty(_validationMessage)) { _validationMessage = await _specialValidationFactory.ValidateMessageAsync(eventData.EventName, message, token); }
                _logWriter.LogInfo("Run data rule validator", "HandleEvent", "Handle");
                if (string.IsNullOrEmpty(_validationMessage)) { _validationMessage = await _dataRuleValidator.ValidateMessageAsync(message, eventData, token); }
                _logWriter.LogInfo("Finish data rule validator", "HandleEvent", "Handle");

                if (string.IsNullOrEmpty(_validationMessage))
                {
                    _logWriter.LogInfo("Validation successful", "HandleEvent", "Handle");
                }
                else
                {
                    return new IdModel();
                }

                // Get workflow state

                if (!string.IsNullOrEmpty(command.View) && command.View != "instrumenterror")
                {
                    if (eventData.UsesSpecimenWorkflowResolution())
                    {
                        _logWriter.LogInfo("Get the next workflow state for event", "HandleEvent", "Handle");
                        command = await _workflowHandler.GetNextStateAsync(message, command, eventData, token);
                        if (!string.IsNullOrEmpty(command.StateId) && command.NewStateId == "")
                        {
                            _validationMessage = "Record not in the correct state to carry out this action";
                            _logWriter.LogInfo(_validationMessage, "HandleEvent", "Handle");
                            return new IdModel();
                        }
                    }
                    else
                    {
                        _logWriter.LogInfo(
                            $"Skipping specimen workflow for event {command.Event}, table {eventData.TableName}, view {command.View}",
                            "HandleEvent",
                            "Handle");
                    }
                }

                // Store the event in the event queue

                if (!eventData.DoNotSaveInQueue)
                {
                    _logWriter.LogInfo("Add the event data into the queue", "HandleEvent", "Handle");
                    queueId = await _messageQueue.AddAsync(message, token.Username, eventData);
                    _logWriter.LogInfo($"Event added to the queue with id : {queueId}", "HandleEvent", "Handle");
                }

                // Carry out any mappings

                if (!string.IsNullOrEmpty(eventData.Mapping))
                {
                    _logWriter.LogInfo("Map event data for event", "HandleEvent", "Handle");
                    var mapperDef = await _mapperAdapter.GetMapperAsync(eventData.Mapping);
                    message = await _processMapping.ProcessAsync(mapperDef, message);
                }

                var specimenPatientData = new SpecimenPatientModel();

                // Handle the event
                var formattedMessage = message;
                if (!TryApplyBillingLaboratoryFromToken(ref formattedMessage, eventData.TableName, token, out var billingLabError))
                {
                    _validationMessage = billingLabError;
                    _logWriter.LogInfo(billingLabError, "HandleEvent", "Handle");
                    if (queueId > 0)
                    {
                        await _messageQueue.UpdateStatusAsync(queueId, 667, 0, eventData.TableName, 0, billingLabError, eventData.EventName, specimenPatientData);
                    }

                    return new IdModel();
                }

                _genericRepository.AddConfiguration(eventData.TableName);
                var recordId = new IdModel();
                var returnId = 0;
                var tracksSaveResult = false;
                var eventTypeLower = eventData.EventType.ToLower();

                if (eventTypeLower == "deletedata" || eventData.EventName == "DeleteCulture" || eventData.EventName == "deletecomment" || eventData.EventName.ToLower() == "deleteisolateevent")
                {
                    recordId = JsonConvert.DeserializeObject<IdModel>(message);

                    specimenPatientData = await _messageQueue.GetSpecimenPatientDetailsAsync(eventData.TableName, eventData.EventName, int.Parse(recordId.Id));
                }

                IRun factory;
                switch (eventTypeLower)
                {
                    case "adddata":
                        _logWriter.LogInfo("Adding data using the generic handler", "HandleEvent", "Handle");
                        tracksSaveResult = true;
                        returnId = await _genericRepository.AddAsync(formattedMessage, command, eventData.StringFields);
                        recordId.Id = returnId.ToString();
                        break;
                    case "editdata":
                        _logWriter.LogInfo("Editing data using the generic handler", "HandleEvent", "Handle");
                        recordId = JsonConvert.DeserializeObject<IdModel>(message);
                        await _genericRepository.EditAsync(formattedMessage, recordId.Id, command, eventData.StringFields);
                        break;
                    case "deletedata":
                        _logWriter.LogInfo("Deleting data using the generic handler", "HandleEvent", "Handle");
                        await _genericRepository.DeleteAsync(recordId.Id, command);
                        break;
                    case "special":
                        _logWriter.LogInfo("Calling the special event handler", "HandleEvent", "Handle");
                        recordId = JsonConvert.DeserializeObject<IdModel>(message);
                        factory = _specialEventFactory.GetEvent(eventData.EventName, token);
                        await factory.RunAsync(formattedMessage, recordId.Id, command, eventData);
                        break;
                    case "specialadddata":
                        _logWriter.LogInfo("Adding data using the special event handler", "HandleEvent", "Handle");
                        tracksSaveResult = true;
                        factory = _specialEventFactory.GetEvent(eventData.EventName, token);
                        returnId = await factory.RunAsync(formattedMessage, "0", command, eventData);
                        recordId.Id = returnId.ToString();
                        AddParentRecordIds(recordId, factory);
                        break;
                }

                if (tracksSaveResult && returnId.IsFailedSaveId())
                {
                    if (eventData.Topic.IsSameAs("Configuration"))
                    {
                        _logWriter.LogInfo(
                            $"Configuration event {eventData.EventName} returned 0; not treating it as a failed specimen save",
                            nameof(HandleEvent),
                            nameof(HandleAsync));
                    }
                    else
                    {
                        return await HandleFailedSaveAsync(queueId, eventData, specimenPatientData, "handler returned id 0");
                    }
                }

                int number;
                // Raise Alerts

                if (recordId != null && recordId.Id != null && recordId.Id != "0")
                {
                    recordId.Id = recordId.Id.Contains("|") ? recordId.Id.Split("|")[0] : recordId.Id;
                    if (eventData.EventType.ToLower() != "deletedata" && eventData.EventName != "DeleteCulture" && eventData.EventName != "deletecomment" && eventData.EventName.ToLower() != "deleteisolateevent")
                    {
                        if (int.TryParse(recordId.Id, out number))
                        {
                            specimenPatientData = await _messageQueue.GetSpecimenPatientDetailsAsync(eventData.TableName, eventData.EventName, int.Parse(recordId.Id));
                        }
                    }
                    if (specimenPatientData != null && specimenPatientData.SpecimenId > 0)
                    {
                        _logWriter.LogInfo($"Raise any alerts on the event for specimen : {specimenPatientData.SpecimenId}", "HandleEvent", "Handle");
                        await _alertHandler.RaiseAlertAsync(specimenPatientData.SpecimenId);
                    }
                    else if (specimenPatientData != null && specimenPatientData.SpecimenId == 0
                        && (eventData.EventName.IsSameAs("culturecommentevent")
                            || eventData.EventName.IsSameAs("specimencomment")
                            || eventData.EventName.IsSameAs("editcomment")))
                    {
                        _logWriter.LogInfo(
                            $"Alert lookup returned no specimen context for event {eventData.EventName}, recordId {recordId.Id}",
                            "HandleEvent",
                            "Handle");
                    }
                }

                if (specimenPatientData == null) { return recordId; }

                var newStateId = string.IsNullOrEmpty(command.NewStateId) ? 0 : int.Parse(command.NewStateId);
                var queueRecordId = recordId != null && recordId.Id != null && int.TryParse(recordId.Id, out number) ? int.Parse(recordId.Id) : 0;
                _logWriter.LogInfo("Update final event status", "HandleEvent", "Handle");
                await _messageQueue.UpdateStatusAsync(queueId, 666, queueRecordId, eventData.TableName, newStateId, "", eventData.EventName, specimenPatientData);

                // Handle actions

                var actions = _workflowHandler.GetActions(command, message);
                if (actions.Count != 0)
                {
                    foreach(var action in actions)
                    {
                        await _actionHandler.CarryOutAction(action, formattedMessage, recordId, token);
                    }
                }

                return recordId;
            }
            catch (ArgumentException e)
            {
                _logWriter.LogError($"Validation error during event : {e.Message}", "HandleEvent", "Handle");
                _validationMessage = e.Message;
                if (queueId > 0 && eventData != null)
                {
                    await _messageQueue.UpdateStatusAsync(queueId, 667, 0, eventData.TableName, 0, e.Message, eventData.EventName, new SpecimenPatientModel());
                }

                return new IdModel { Id = "" };
            }
            catch (TransactionAbortedException e)
            {
                var hint = e.InnerException?.Message?.Contains("prepared transactions", StringComparison.OrdinalIgnoreCase) == true
                    ? " Hint: ensure MoreData merges use the caller's shared NpgsqlConnection inside TransactionScope."
                    : string.Empty;
                if (queueId > 0 && eventData != null)
                {
                    _logWriter.LogError(
                        $"Transaction aborted during event: event={eventData.EventName}, table={eventData.TableName}, queueId={queueId}, topic={eventData.Topic}, detail={e}{hint}",
                        nameof(HandleEvent),
                        nameof(HandleAsync));
                    await _messageQueue.UpdateStatusAsync(queueId, 667, 0, eventData.TableName, 0, e.Message, eventData.EventName, new SpecimenPatientModel());
                    _validationMessage = SaveFailureLanguageTags.GenericSaveFailure;
                }
                else
                {
                    _logWriter.LogError($"Transaction aborted during event : {e}{hint}", "HandleEvent", "Handle");
                    _validationMessage = "A problem happened while handling your request.";
                }

                return new IdModel { Id = "" };
            }
            catch (Exception e)
            {
                if (queueId > 0 && eventData != null)
                {
                    _logWriter.LogError(
                        $"Error occurred during event: event={eventData.EventName}, table={eventData.TableName}, queueId={queueId}, topic={eventData.Topic}, detail={e}",
                        nameof(HandleEvent),
                        nameof(HandleAsync));
                    await _messageQueue.UpdateStatusAsync(queueId, 667, 0, eventData.TableName, 0, e.Message, eventData.EventName, new SpecimenPatientModel());
                    _validationMessage = SaveFailureLanguageTags.GenericSaveFailure;
                }
                else
                {
                    _logWriter.LogError($"Error occurred during event : {e}", "HandleEvent", "Handle");
                    _validationMessage = "A problem happened while handling your request.";
                }

                return new IdModel { Id = "" };
            }

        }

        /// <summary>
        /// Records a failed save against the event queue and prepares the user-facing validation message.
        /// </summary>
        /// <param name="queueId">Queue entry created before the save attempt.</param>
        /// <param name="eventData">Configuration for the event being processed.</param>
        /// <param name="specimenPatientData">Workflow context for queue status update.</param>
        /// <param name="detail">Diagnostic detail written to logs and the queue error column.</param>
        /// <returns>An empty id model; <see cref="GetValidationMessage"/> carries <see cref="SaveFailureLanguageTags.GenericSaveFailure"/>.</returns>
        private async Task<IdModel> HandleFailedSaveAsync(int queueId, EventConfig eventData, SpecimenPatientModel specimenPatientData, string detail)
        {
            _validationMessage = SaveFailureLanguageTags.GenericSaveFailure;
            _logWriter.LogError(
                $"Save failed: event={eventData.EventName}, table={eventData.TableName}, queueId={queueId}, topic={eventData.Topic}, detail={detail}",
                nameof(HandleEvent),
                nameof(HandleAsync));
            if (queueId > 0)
            {
                _logWriter.LogInfo(
                    $"Updating queue {queueId} to Failed (667) for event {eventData.EventName}",
                    nameof(HandleEvent),
                    nameof(HandleAsync));
                await _messageQueue.UpdateStatusAsync(queueId, 667, 0, eventData.TableName, 0, detail, eventData.EventName, specimenPatientData);
            }

            return new IdModel { Id = "" };
        }

        /// <summary>
        /// Copies the parent record ids onto the response for an event that creates records above the one it
        /// returns. A form that offers to run again uses them to attach the next record to the same parents.
        /// </summary>
        /// <param name="recordId">Response being built for the client.</param>
        /// <param name="handler">The event handler that has just run.</param>
        private static void AddParentRecordIds(IdModel recordId, IRun handler)
        {
            if (handler is not ICreateParentRecords parents)
            {
                return;
            }

            recordId.PatientId = parents.CreatedPatientId.ToString();
            recordId.AdmissionId = parents.CreatedAdmissionId?.ToString();
            recordId.RequestId = parents.CreatedRequestId?.ToString();
        }

        /// <summary>
        /// Gets the validation message.
        /// </summary>
        public string GetValidationMessage()
        {
            return _validationMessage;
        }

        /// <summary>
        /// For laboratory-scoped billing tables, LaboratoryId must come only from the auth token, never the client payload.
        /// </summary>
        private static bool TryApplyBillingLaboratoryFromToken(ref string formattedMessage, string tableName, TokenInfoModel token, out string error)
        {
            error = null;
            if (string.IsNullOrEmpty(tableName))
                return true;

            if (!tableName.Equals("BillingRule", StringComparison.OrdinalIgnoreCase)
                && !tableName.Equals("BillingRecord", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.IsNullOrWhiteSpace(token?.LaboratoryId) || !int.TryParse(token.LaboratoryId.Trim(), out var labId))
            {
                error = "Laboratory or Organisation not supplied";
                return false;
            }

            JObject jo;
            try
            {
                jo = JObject.Parse(formattedMessage);
            }
            catch (JsonReaderException)
            {
                error = "Invalid event payload.";
                return false;
            }

            foreach (var prop in jo.Properties().Where(p => p.Name.Equals("LaboratoryId", StringComparison.OrdinalIgnoreCase)).ToList())
                prop.Remove();

            jo["LaboratoryId"] = labId;
            formattedMessage = jo.ToString(Formatting.None);
            return true;
        }
    }
}
