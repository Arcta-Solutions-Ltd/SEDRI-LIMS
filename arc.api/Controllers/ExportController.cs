using arc.app.Exports;
using arc.app.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using arc.app.Common;
using arc.domain.Configuration.QueryFiltersConfig;

namespace arc.api.Controllers;

/// <summary>
/// Controller for handling export operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ExportController(
    ILogWriter logWriter,
    ITokenHandler tokenHandler,
    IExportRunHandler exportRunHandler,
    ILanguageHandler languageHandler,
    IControllerUtils controllerUtils)
    : BaseController(logWriter, controllerUtils, tokenHandler)
{
    private readonly ILogWriter _logger = logWriter;
    private readonly IExportRunHandler _exportRunHandler = exportRunHandler;
    private readonly ILanguageHandler _languageHandler = languageHandler;

    /// <summary>
    /// Runs the export operation asynchronously.
    /// </summary>
    /// <returns>An ActionResult indicating the outcome of the operation.</returns>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("run")]
    [HttpPost]
    public async Task<ActionResult> RunExportAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, token, queryFilters) = await AuthorizeAndGetContentAsync<QueryFilterConfig>();
            if (authResult != null) { return authResult; }

            var startDateString = queryFilters.GetStringValue("startdate");
            var startDate = !string.IsNullOrEmpty(startDateString) ? DateTime.Parse(startDateString) : new DateTime();
            var endDateString = queryFilters.GetStringValue("endDate");
            var endDate = !string.IsNullOrEmpty(endDateString) ? DateTime.Parse(endDateString) : new DateTime();
            var errorMessage = "";

            if (string.IsNullOrEmpty(startDateString) || string.IsNullOrEmpty(endDateString))
            {
                errorMessage = "@RunExpE@";
            }

            if (errorMessage == "" && startDate > endDate)
            {
                errorMessage = "@ExpSta@";
            }

            if (errorMessage != "")
            {
                var badRequest = await _languageHandler.TranslateAsync(errorMessage, token.LanguageId);
                _logger.LogInfo(badRequest, nameof(ExportController), nameof(RunExportAsync));
                return BadRequest(badRequest);
            }

            var result = await _exportRunHandler.RunExportAsync(queryFilters, token);

            if (string.IsNullOrEmpty(result))
            {
                return StatusCode(500, "This export contains no data.");
            }

            return Content(result, "application/json"); 
        }, nameof(RunExportAsync));
    }
}
