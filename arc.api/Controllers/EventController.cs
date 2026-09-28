using arc.app.Common;
using arc.app.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace arc.api.Controllers;

/// <summary>
/// Handles API requests related to carrying out events.
/// </summary>
/// <param name="logWriter">The logger to log information.</param>
/// <param name="runEventHandler">Handler to execute the event.</param>
/// <param name="tokenHandler">The token handler for authorization.</param>
/// <param name="controllerUtils">Utilities for controller operations.</param>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class EventController(
    ILogWriter logWriter,
    IRunEventHandler runEventHandler,
    ITokenHandler tokenHandler,
    IControllerUtils controllerUtils)
    : BaseController(logWriter, controllerUtils, tokenHandler)
{
    private readonly IRunEventHandler _runEventHandler = runEventHandler;

    /// <summary>
    /// Handles the POST request to carry out an event and process it accordingly.
    /// </summary>
    /// <returns>An ActionResult indicating the outcome of the operation.</returns>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("post")]
    [HttpPost]
    public async Task<ActionResult> PostAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var authResult = await InitializeAndCheckAuthorizationAsync();
            if (authResult != null) { return authResult; }
            var token = _tokenHandler.GetTokenInfo(User);

            var validationMessage = await _runEventHandler.RunAsync(_controllerUtils.GetContents(), token);

            if (string.IsNullOrEmpty(validationMessage))
            {
                return Ok(_runEventHandler.Result);
            }

            // WAPT-006: Log validation and save-failure messages internally. Validation tags are returned
            // to the client; save failures use the generic @SavF@ tag after translation.
            _logWriter.LogError(validationMessage, nameof(EventController), nameof(PostAsync));
            return StatusCode(500, validationMessage);
        }, nameof(PostAsync));
    }
}
