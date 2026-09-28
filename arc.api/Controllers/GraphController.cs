using arc.app.Common;
using arc.app.Graph;
using arc.app.Security;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace arc.api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class GraphController(
    ILogWriter logWriter,
    ITokenHandler tokenHandler,
    IGraphFactory graphFactory,
    ILanguageHandler languageHandler,
    IStandardFilters standardFilters,
    IControllerUtils controllerUtils)
    : BaseController(logWriter, controllerUtils, tokenHandler)
{
    private readonly IGraphFactory _graphFactory = graphFactory;
    private readonly ILanguageHandler _languageHandler = languageHandler;
    private readonly IStandardFilters _standardFilters = standardFilters;

    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    [Route("getdata")]
    [HttpPost]
    public async Task<ActionResult> GetData()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var (authResult, token, queryFilters) = await AuthorizeAndGetContentAsync<QueryFilterConfig>();
            if (authResult != null) { return authResult; }

            GraphDashboardTimeRangeResolver.Apply(queryFilters, logWriter);

            queryFilters.AddTokenFilter("specimen", token);
            // Preserve existing behavior: do not apply standard filters here
            var graphData = await _graphFactory.GetGraphDataAsync(queryFilters, token);
            graphData = await _languageHandler.TranslateAsync(graphData, token.LanguageId);

            logWriter.LogInfo(
                $"Graph getdata completed for query '{queryFilters.Name ?? "?"}' (payload length {graphData?.Length ?? 0}).",
                nameof(GraphController),
                nameof(GetData));

            return Content(graphData, "application/json");
        }, nameof(GetData));
    }
}
