using arc.app.Alert;
using arc.app.Common;
using arc.app.Security;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace arc.api.Controllers;

/// <summary>
/// API controller for retrieving alert messages for the authenticated user.
/// </summary>
/// <remarks>
/// Follows the standardized controller pattern using centralized monitoring and
/// authorization helpers from <see cref="BaseController"/>.
/// </remarks>

[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class AlertController(
    ILogWriter logWriter,
    IControllerUtils controllerUtils,
    ITokenHandler tokenHandler,
    IAlertMessageHandler alertMessageHandler)
    : BaseController(logWriter, controllerUtils, tokenHandler)
{
    private readonly IAlertMessageHandler _alertMessageHandler = alertMessageHandler;

    [Route("get")]
    [HttpPost]
    /// <summary>
    /// Gets alert messages based on supplied query filters.
    /// </summary>
    /// <returns>
    /// 200 OK with alert messages on success, 401 Unauthorized if not authenticated,
    /// 400 Bad Request when required parameters are missing.
    /// </returns>
    public async Task<ActionResult> Get()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, _, queryFilters) = await AuthorizeAndGetContentAsync<QueryFilterConfig>();
            if (authResult != null) { return authResult; }

            _logWriter.LogInfo("Call alert handler to get the alert messages", nameof(AlertController), nameof(Get));
            var result = await _alertMessageHandler.GetMessagesAsync(queryFilters);

            return Ok(result);
        }, nameof(Get));
    }
}
