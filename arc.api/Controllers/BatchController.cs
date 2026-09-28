using arc.app.Common;
using arc.app.Reports;
using arc.app.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace arc.api.Controllers;

/// <summary>
/// Controller for handling batch print operations.
/// </summary>
/// <param name="logWriter">Logger for capturing and recording error and event information.</param>
/// <param name="tokenHandler">Provides token-based user information and access control.</param>
/// <param name="controllerUtils">Utility functions for controller operations like content retrieval.</param>
/// <param name="batchHandler">Handles batch printing logic and orchestration.</param>
/// <param name="languageHandler">Translates output based on user language preferences.</param>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class BatchController(
    ILogWriter logWriter,
    ITokenHandler tokenHandler,
    IControllerUtils controllerUtils,
    IBatchPrintHandler batchHandler,
    ILanguageHandler languageHandler)
    : BaseController(logWriter, controllerUtils, tokenHandler)
{
    private readonly IBatchPrintHandler _batchhandler = batchHandler;
    private readonly ILanguageHandler _languageHandler = languageHandler;

    /// <summary>
    /// POST endpoint to initiate a batch print process.
    /// </summary>
    /// <remarks>
    /// Validates user authorization and token tags. Handles batch logic and returns a translated result.
    /// </remarks>
    /// <returns>
    /// 200 OK with translated result if successful, 401 Unauthorized if token lacks required tag.
    /// </returns>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("print")]
    [HttpPost]
    public async Task<ActionResult> PrintAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var authResult = await InitializeAndCheckAuthorizationAsync();
            if (authResult != null) { return authResult; }
            var token = _tokenHandler.GetTokenInfo(User);

            if (!token.Tags.Contains("AR")) { return Unauthorized(); }

            var result = await _batchhandler.HandleAsync(_controllerUtils.GetContents(), token);

            var translated = await _languageHandler.TranslateAsync(result, token.LanguageId);
            return Ok(translated);
        }, nameof(PrintAsync));
    }
}
