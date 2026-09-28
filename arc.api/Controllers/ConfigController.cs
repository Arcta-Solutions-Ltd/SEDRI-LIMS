using arc.app.Common;
using arc.app.Config;
using arc.app.Config.Events;
using arc.app.Config.Reports;
using arc.app.Configuration;
using arc.app.Reports;
using arc.app.Reports.ReportDesigner;
using arc.app.Security;
using arc.common;
using arc.common.Models.Common;
using arc.common.Models.Config;
using arc.common.Models.Reports;
using arc.common.Models.Reports.ReportDesigner;
using arc.common.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace arc.api.Controllers;

/// <summary>
/// Controller for configuration management operations including system configuration,
/// report definitions, configuration export, and report designer functionality.
/// </summary>
/// <remarks>
/// This controller provides endpoints for:
/// - Retrieving system configuration data
/// - Getting report definitions with translation
/// - Exporting configuration data
/// - Accessing report designer configurations
///
/// All endpoints require authentication and automatically handle language translation
/// based on the user's language preferences.
/// </remarks>
///
[ApiController]
[Authorize]
[Route("api/config")]
[Produces("application/json")]
public class ConfigController(
    IHandleConfig configHandler,
    ILogWriter logWriter,
    ITokenHandler tokenHandler,
    IReportAdapter reportAdapter,
    ILanguageHandler languageHandler,
    IReportTranslator reportTranslator,
    IConfigTransferHandler configTransferHandler,
    IControllerUtils controllerUtils,
    IReportDesignerHandler reportDesignerHandler,
    IWorkflowDesignerHandler workflowDesignerHandler,
    ISpecialEventFactory specialEventFactory)
    : BaseController(logWriter, controllerUtils, tokenHandler)
{
    private readonly IHandleConfig _configHandler = configHandler;
    private readonly IReportAdapter _reportAdapter = reportAdapter;
    private readonly ILanguageHandler _languageHandler = languageHandler;
    private readonly IReportTranslator _reportTranslator = reportTranslator;
    private readonly IConfigTransferHandler _configTransferHandler = configTransferHandler;
    private readonly IReportDesignerHandler _reportDesignerHandler = reportDesignerHandler;
    private readonly IWorkflowDesignerHandler _workflowDesignerHandler = workflowDesignerHandler;
    private readonly ISpecialEventFactory _specialEventFactory = specialEventFactory;

    /// <summary>
    /// Gets the configuration data for the authenticated user.
    /// </summary>
    /// <returns>
    /// An ActionResult containing the configuration data.
    /// Returns 200 OK with the configuration data if successful,
    /// 401 Unauthorized if the user lacks proper authorization.
    /// </returns>
    /// <remarks>
    /// This endpoint retrieves the complete system configuration including
    /// user preferences, system settings, and localized content based on the user's language.
    /// </remarks>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [Route("get")]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var token = _tokenHandler.GetTokenInfo(User);
            var authResult = await InitializeAndCheckAuthorizationAsync();
            if (authResult != null) { return authResult; }

            var result = await _configHandler.GetConfigurationAsync(token, token.LanguageId);

            return Content(result, "application/json");
        }, nameof(GetAsync));
    }

    /// <summary>
    /// Gets a report based on the provided report model.
    /// </summary>
    /// <returns>
    /// An ActionResult containing the report data as a translated JSON string.
    /// Returns 200 OK with the report data if successful,
    /// 401 Unauthorized if the user lacks proper authorization,
    /// 400 Bad Request if the request data is invalid.
    /// </returns>
    /// <remarks>
    /// This endpoint retrieves a complete report definition including all sections,
    /// formatting, and configuration. The response is automatically translated
    /// based on the user's language preference.
    /// </remarks>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("getreport")]
    [HttpPost]
    public async Task<ActionResult> GetReportAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, token, report) = await AuthorizeAndGetContentAsync<ReportModel>();
            if (authResult != null) { return authResult; }

            // Validate input
            var validationResult = ValidateRequiredParameter(report?.ReportName, "Report name", nameof(GetReportAsync));
            if (validationResult != null) { return validationResult; }

            var def = await _reportAdapter.GetReportAsync(report.ReportName);
            var reportDefinitionToSend = await _reportTranslator.TranslateAsync(def);
            var result = await SerializeAndTranslateAsync(reportDefinitionToSend, token.LanguageId, _languageHandler);

            return CreateSuccessResponse(result, nameof(GetReportAsync),
                $"Successfully retrieved report: {report.ReportName}");
        }, nameof(GetReportAsync));
    }

    /// <summary>
    /// Exports the configuration data based on the provided export settings.
    /// </summary>
    /// <returns>
    /// An ActionResult containing the exported configuration data.
    /// Returns 200 OK with the exported configuration if successful,
    /// 401 Unauthorized if the user lacks proper authorization,
    /// 400 Bad Request if the request data is invalid.
    /// </returns>
    /// <remarks>
    /// This endpoint exports system configuration data according to the specified
    /// export settings, allowing for backup, migration, or analysis purposes.
    /// </remarks>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("export")]
    [HttpPost]
    public async Task<ActionResult> ExportAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, _, configSettings) = await AuthorizeAndGetContentAsync<ConfigurationExportModel>();
            if (authResult != null) { return authResult; }

            var result = await _configTransferHandler.GetConfig(configSettings);

            return Content(result, "application/json");
        }, nameof(ExportAsync));
    }

    /// <summary>
    /// Gets the report designer configuration for the specified report.
    /// </summary>
    /// <returns>
    /// An ActionResult containing the report designer configuration as a translated JSON string.
    /// Returns 200 OK with the configuration data if successful,
    /// 401 Unauthorized if the user lacks proper authorization,
    /// 400 Bad Request if the request data is invalid.
    /// </returns>
    /// <remarks>
    /// This endpoint retrieves the complete report designer configuration including
    /// categories, section definitions, and available datasections for the specified report.
    /// The response is automatically translated based on the user's language preference.
    /// </remarks>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("getreportdesignerconfig")]
    [HttpPost]
    public async Task<ActionResult> GetReportDesignerConfigAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, token, report) = await AuthorizeAndGetContentAsync<IdAndNameModel>();
            if (authResult != null) { return authResult; }

            // Validate input
            var validationResult = ValidateRequiredParameter(report?.Name, "Report name", nameof(GetReportDesignerConfigAsync));
            if (validationResult != null) { return validationResult; }

            var config = await _reportDesignerHandler.GetReportDesignerConfiguration(report);
            var result = await SerializeAndTranslateAsync(config, token.LanguageId, _languageHandler);

            return CreateSuccessResponse(result, nameof(GetReportDesignerConfigAsync),
                $"Successfully retrieved report designer configuration for report: {report.Name}");
        }, nameof(GetReportDesignerConfigAsync));
    }

    /// <summary>
    /// Saves the report designer configuration changes to the database.
    /// </summary>
    /// <returns>
    /// An ActionResult containing the save operation result.
    /// Returns 200 OK with success message if successful,
    /// 401 Unauthorized if the user lacks proper authorization,
    /// 400 Bad Request if the request data is invalid.
    /// </returns>
    /// <remarks>
    /// This endpoint saves report configuration changes including report definitions,
    /// section definitions, and custom formats. All changes are saved in a single transaction
    /// to ensure data consistency. The response describes every configuration record that was
    /// written, so the designer can reconcile any name the backend had to change.
    /// </remarks>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("savereportdesignerconfig")]
    [HttpPost]
    public async Task<ActionResult> SaveReportDesignerConfigAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, token, config) = await AuthorizeAndGetContentAsync<SaveReportDesignerConfigModel>();
            if (authResult != null) { return authResult; }

            // Validate input
            var validationResult = ValidateRequiredParameter(config?.Name, "Report name", nameof(SaveReportDesignerConfigAsync));
            if (validationResult != null) { return validationResult; }

            _logWriter.LogInfo(
                $"Received report designer save for '{config.Name}': {config.ChangedCustomFormats?.Count ?? 0} format(s), {config.ChangedSectionDefinitions?.Count ?? 0} section(s), {config.DeletedFormats?.Count ?? 0} format deletion(s), {config.DeletedSections?.Count ?? 0} section deletion(s), {config.Categories?.Count ?? 0} categor(y/ies).",
                nameof(ConfigController), nameof(SaveReportDesignerConfigAsync));

            // Get the event handler from the factory
            var eventHandler = _specialEventFactory.GetEvent("savereportdesignerconfig", token);
            if (eventHandler == null)
            {
                return CreateErrorResponse("Event handler not found", nameof(SaveReportDesignerConfigAsync));
            }

            // Serialize the config back to JSON for the event handler
            var serializedData = await SerializeAndTranslateAsync(config, token.LanguageId, _languageHandler);
            var eventModel = new EventModel { Event = "savereportdesignerconfig" };

            // Execute the save operation
            await eventHandler.RunAsync(serializedData, config.Name, eventModel);

            var saveResult = (eventHandler as IProvideSaveResult<ReportDesignerSaveResultModel>)?.SaveResult
                             ?? new ReportDesignerSaveResultModel { ReportName = config.Name };

            return CreateSuccessResponse(ArcJson.Serialize(saveResult), nameof(SaveReportDesignerConfigAsync),
                $"Successfully saved report designer configuration for report: {config.Name}");
        }, nameof(SaveReportDesignerConfigAsync));
    }

    /// <summary>
    /// Gets the workflow designer configuration for the specified workflow.
    /// </summary>
    /// <returns>
    /// An ActionResult containing the workflow designer configuration as a translated JSON string.
    /// Returns 200 OK with the configuration data if successful,
    /// 401 Unauthorized if the user lacks proper authorization,
    /// 400 Bad Request if the request data is invalid.
    /// </returns>
    /// <remarks>
    /// The workflow is located by its configs identity, so the designer always edits the record the
    /// user picked. The response carries the stored workflow document plus read-only state, event and
    /// list lookups, translated for the user's language.
    /// </remarks>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("getworkflowdesignerconfig")]
    [HttpPost]
    public async Task<ActionResult> GetWorkflowDesignerConfigAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, token, workflow) = await AuthorizeAndGetContentAsync<IdAndNameModel>();
            if (authResult != null) { return authResult; }

            var validationResult = ValidateRequiredParameter(workflow?.Id, "Workflow id", nameof(GetWorkflowDesignerConfigAsync));
            if (validationResult != null) { return validationResult; }

            _logWriter.LogInfo(
                $"Received workflow designer load for id '{workflow.Id}' ('{workflow.Name}').",
                nameof(ConfigController), nameof(GetWorkflowDesignerConfigAsync));

            var config = await _workflowDesignerHandler.GetWorkflowDesignerConfigurationAsync(workflow);
            var result = await SerializeAndTranslateAsync(config, token.LanguageId, _languageHandler);

            return CreateSuccessResponse(result, nameof(GetWorkflowDesignerConfigAsync),
                $"Successfully retrieved workflow designer configuration for workflow: {workflow.Name}");
        }, nameof(GetWorkflowDesignerConfigAsync));
    }

    /// <summary>
    /// Saves the workflow designer changes to the database.
    /// </summary>
    /// <returns>
    /// An ActionResult containing the save operation result.
    /// Returns 200 OK with the save result if successful,
    /// 401 Unauthorized if the user lacks proper authorization,
    /// 400 Bad Request if the request data is invalid.
    /// </returns>
    /// <remarks>
    /// The whole workflow document is written in a single transaction, matched on the configs identity
    /// the designer loaded. Only the workflow is written; the supporting lookups are reference data.
    /// </remarks>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("saveworkflowdesignerconfig")]
    [HttpPost]
    public async Task<ActionResult> SaveWorkflowDesignerConfigAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, token, config) = await AuthorizeAndGetContentAsync<SaveWorkflowDesignerConfigModel>();
            if (authResult != null) { return authResult; }

            var validationResult = ValidateRequiredParameter(config?.ConfigName, "Workflow name", nameof(SaveWorkflowDesignerConfigAsync));
            if (validationResult != null) { return validationResult; }

            _logWriter.LogInfo(
                $"Received workflow designer save for '{config.ConfigName}' (id {config.ConfigId}).",
                nameof(ConfigController), nameof(SaveWorkflowDesignerConfigAsync));

            var eventHandler = _specialEventFactory.GetEvent("saveworkflowdesignerconfig", token);
            if (eventHandler == null)
            {
                return CreateErrorResponse("Event handler not found", nameof(SaveWorkflowDesignerConfigAsync));
            }

            // The workflow document is stored verbatim, so it must not be run through the language
            // handler on the way in; translating it would replace the @Tag@ tokens it carries.
            var serializedData = ArcJson.Serialize(config);
            var eventModel = new EventModel { Event = "saveworkflowdesignerconfig" };

            await eventHandler.RunAsync(serializedData, config.ConfigName, eventModel);

            var saveResult = (eventHandler as IProvideSaveResult<WorkflowDesignerSaveResultModel>)?.SaveResult
                             ?? new WorkflowDesignerSaveResultModel();

            return CreateSuccessResponse(ArcJson.Serialize(saveResult), nameof(SaveWorkflowDesignerConfigAsync),
                $"Successfully saved workflow designer configuration for workflow: {config.ConfigName}");
        }, nameof(SaveWorkflowDesignerConfigAsync));
    }
}
