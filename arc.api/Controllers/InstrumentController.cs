
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using arc.app.Security;
using System.Threading.Tasks;
using arc.app.Common;
using arc.app.Instruments;
using Newtonsoft.Json;
using arc.common.Models.Instruments;

namespace arc.api.Controllers;

/// <summary>
/// Handles API requests related to passing data to the instrument interface.
/// </summary>
/// <param name="logWriter">The logger to log information.</param>
/// <param name="controllerUtils">The controller utilities.</param>
/// <param name="tokenHandler">The token handler for authorization.</param>
/// <param name="instrumentRequestHandler">The handler managing requests to the interface.</param>
/// <param name="instrumentResultHandler">The handler managing results sent through the interface.</param>
/// <param name="logger">Structured logger for instrument API diagnostics.</param>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class InstrumentController(
    ILogWriter logWriter,
    IControllerUtils controllerUtils,
    ITokenHandler tokenHandler,
    IInstrumentRequestHandler instrumentRequestHandler,
    IInstrumentResultHandler instrumentResultHandler,
    ILogger<InstrumentController> logger)
    : BaseController(logWriter, controllerUtils, tokenHandler)
{
    private readonly IInstrumentRequestHandler _instrumentRequestHandler = instrumentRequestHandler;
    private readonly IInstrumentResultHandler _instrumentResultHandler = instrumentResultHandler;
    private readonly ILogger<InstrumentController> _logger = logger;

    /// <summary>
    /// Receives an interface record posted from the instrument interface application.
    /// Optional <c>SourceFileAttachmentIds</c> on the body lists <c>fileattachments</c> ids (from <c>POST api/file/upload</c>) to link as source files for the instrument result.
    /// Optional <c>InstrumentMachineId</c> may be set for audit/correlation (InstrumentMachine list item id, same as MIS appsettings <c>InstrumentId</c>).
    /// </summary>
    /// <returns>An ActionResult indicating the outcome of the operation.</returns>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Route("post")]
    [HttpPost]
    public async Task<ActionResult> PostAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, token, content) = await AuthorizeAndGetContentAsync<ResponseModel>();
            if (authResult != null) { return authResult; }

            var success = await _instrumentResultHandler.HandleAsync(content, token);
            return success ? Ok() : BadRequest();
        }, nameof(PostAsync));
    }

    /// <summary>
    /// Confirms that outbound instrument work has been processed for a pending <c>instrumentresults</c> row (status moves to Requested).
    /// Optional <c>SourceFileAttachmentIds</c> lists <c>fileattachments</c> ids from <c>POST api/file/upload</c> to link as traceability files for that result (same junction table as inbound <c>POST api/instrument/post</c>).
    /// </summary>
    /// <returns>An ActionResult indicating the outcome of the operation.</returns>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [Route("confirm")]
    [HttpPost]
    public async Task<ActionResult> ConfirmRequestsReceivedAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, _, content) = await AuthorizeAndGetContentAsync<RequestConfirmModel>();
            if (authResult != null) { return authResult; }

            var attachmentCount = content.SourceFileAttachmentIds?.Count ?? 0;
            _logger.LogInformation(
                "Instrument confirm: instrumentResultId={InstrumentResultId}, sourceFileAttachmentIdCount={AttachmentCount}",
                content.Id,
                attachmentCount);

            await _instrumentRequestHandler.ConfirmRequests(content);
            return Ok();
        }, nameof(ConfirmRequestsReceivedAsync));
    }

    /// <summary>
    /// Notifies the system that an error has occurred in the interface.
    /// </summary>
    /// <returns>An ActionResult indicating the outcome of the operation.</returns>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [Route("error")]
    [HttpPost]
    public async Task<ActionResult> ErrorAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, _, content) = await AuthorizeAndGetContentAsync<RequestConfirmModel>();
            if (authResult != null) { return authResult; }

            var errorInfo = JsonConvert.DeserializeObject<InstrumentErrorModel>(ContentsAsString);
            await _instrumentRequestHandler.SaveErrorAsync(errorInfo);
            return Ok();
        }, nameof(ErrorAsync));
    }
}
