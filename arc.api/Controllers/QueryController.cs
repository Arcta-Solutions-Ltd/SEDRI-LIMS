using arc.app.Common;
using arc.app.Config.Queries;
using arc.app.Graph;
using arc.app.Security;
using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace arc.api.Controllers;

/// <summary>
/// API controller for handling queries.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class QueryController(
    ILogWriter logWriter,
    IControllerUtils controllerUtils,
    ITokenHandler tokenHandler,
    IQueryHandlerFactory queryHandlerFactory,
    IQueryAdapter queryAdapter,
    IHandleQuery queryHandler,
    ILanguageHandler languageHandler)
    : BaseController(logWriter, controllerUtils, tokenHandler)
{
    private readonly IQueryHandlerFactory _queryHandlerFactory = queryHandlerFactory;
    private readonly IQueryAdapter _queryAdapter = queryAdapter;
    private readonly IHandleQuery _queryHandler = queryHandler;
    private readonly ILanguageHandler _languageHandler = languageHandler;

    /// <summary>
    /// Handles the GET request to retrieve query results.
    /// </summary>
    /// <param name="queryName">The name of the query to execute.</param>
    /// <returns>An <see cref="ActionResult"/> containing the query results.</returns>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("get")]
    [HttpGet]
    public async Task<ActionResult> GetAsync(string queryName)
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var token = _tokenHandler.GetTokenInfo(User);
            var authResult = await InitializeAndCheckAuthorizationAsync();
            if (authResult != null) { return authResult; }

            var validationResult = ValidateRequiredParameter(queryName, "Query name", nameof(GetAsync));
            if (validationResult != null) { return validationResult; }

            var queryFilters = new QueryFilterConfig { Name = queryName };
            var queryData = await _queryAdapter.GetQueryAsync(queryFilters.Name);

            if (!string.IsNullOrEmpty(queryData?.Tags))
            {
                if (string.IsNullOrEmpty(token.Tags) || !queryData.Tags.ContainsAnyItem(token.Tags))
                {
                    return Unauthorized();
                }
            }

            var result = await _queryHandler.HandleAsync(queryName, queryFilters, queryData, token);

            if (queryData.Translate)
            {
                result = await TranslateQueryResultAsync(queryFilters.Name, result, token.LanguageId);
            }

            return Content(result, "application/json");
        }, nameof(GetAsync));
    }

    /// <summary>
    /// Handles the POST request to retrieve filtered query results.
    /// </summary>
    /// <returns>An <see cref="ActionResult"/> containing the query results.</returns>
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("filteredget")]
    [HttpPost]
    public async Task<ActionResult> FilteredGetAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, token, queryFilters) = await AuthorizeAndGetContentAsync<QueryFilterConfig>();
            if (authResult != null) { return authResult; }

            var validationResult = ValidateRequiredParameter(queryFilters?.Name, "Query name", nameof(FilteredGetAsync));
            if (validationResult != null) { return validationResult; }

            GraphDashboardTimeRangeResolver.Apply(queryFilters, logWriter);

            var queryData = await _queryAdapter.GetQueryAsync(queryFilters.Name);

            if (!string.IsNullOrEmpty(queryData?.Tags))
            {
                if (string.IsNullOrEmpty(token.Tags) || !queryData.Tags.ContainsAnyItem(token.Tags))
                {
                    return Unauthorized();
                }
            }

            var queryHandler = _queryHandlerFactory.Create(queryData.Type);
            var result = await queryHandler.HandleAsync(_controllerUtils.GetContents(), queryFilters, queryData, token);

            if (queryData.Translate)
            {
                result = await TranslateQueryResultAsync(queryFilters.Name, result, token.LanguageId);
            }

            if (string.Equals(queryFilters.Name, "OrganisationList", System.StringComparison.OrdinalIgnoreCase))
            {
                var count = 0;
                try
                {
                    var arr = JArray.Parse(result);
                    count = arr?.Count ?? 0;
                }
                catch
                {
                    count = -1;
                }
                _logWriter.LogInfo($"OrganisationList query executed, returned {count} organisations", nameof(QueryController), nameof(FilteredGetAsync));
            }

            return Content(result, "application/json");
        }, nameof(FilteredGetAsync));
    }

    /// <summary>
    /// Translates language tags in a query JSON result and logs the query name when translation fails.
    /// </summary>
    /// <param name="queryName">The query being translated (for diagnostic logging).</param>
    /// <param name="result">JSON string returned by the query handler.</param>
    /// <param name="languageId">The active language id.</param>
    /// <returns>Translated JSON string.</returns>
    private async Task<string> TranslateQueryResultAsync(string queryName, string result, string languageId)
    {
        try
        {
            return await _languageHandler.TranslateJsonAsync(result, languageId);
        }
        catch (System.Exception ex)
        {
            logWriter.LogError(
                $"TranslateJsonAsync failed for query {queryName}: {ex}",
                nameof(QueryController),
                nameof(TranslateQueryResultAsync));
            throw;
        }
    }
}
