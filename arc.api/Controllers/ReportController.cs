using arc.app.Common;
using arc.app.Reports;
using arc.app.Security;
using arc.common.Models.Reports;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace arc.api.Controllers;

/// <summary>
/// API controller for handling report-related requests.
/// </summary>
[ApiController]
[Authorize]
[Route("api/report")]
[Produces("application/json")]
public class ReportController : BaseController
{
    private readonly ISpecimenRecordReportHandler _specimenRecordReportHandler;
    private readonly ILanguageHandler _languageHandler;

    /// <summary>
    /// Constructor for dependency injection.
    /// </summary>
    /// <param name="logWriter">Log writer service.</param>
    /// <param name="controllerUtils">Controller utilities service.</param>
    /// <param name="tokenHandler">Token handler service.</param>
    /// <param name="specimenRecordReportHandler">Specimen record report handler service.</param>
    /// <param name="languageHandler">Language handler for translations.</param>
    public ReportController(ILogWriter logWriter, IControllerUtils controllerUtils, ITokenHandler tokenHandler, ISpecimenRecordReportHandler specimenRecordReportHandler, ILanguageHandler languageHandler)
        : base(logWriter, controllerUtils, tokenHandler)
    {
        _specimenRecordReportHandler = specimenRecordReportHandler;
        _languageHandler = languageHandler;
    }

    /// <summary>
    /// Gets a report based on the provided report model.
    /// </summary>
    /// <returns>An ActionResult containing the report data.</returns>
    [Route("getreportwithamendments")]
    [HttpPost]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> GetReportWithAmendmentsAsync()
    {
        return await ExecuteWithMonitoringAsync(async () =>
        {
            var authResult = await InitializeAndCheckAuthorizationAsync();
            if (authResult != null) { return authResult; }

            var token = _tokenHandler.GetTokenInfo(User);

            var contents = _controllerUtils.GetContents();
            var reportCriteria = JsonConvert.DeserializeObject<ReportModel>(contents);

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("id", reportCriteria.Id);

            if (!token.Tags.Contains("SP"))
            {
                return Unauthorized();
            }

            if (reportCriteria.Value != null)
            {
                var value = reportCriteria.Value;
                var directTestCount = value.DirectTestList?.Count(d => d.PrintOnReport == "Yes") ?? 0;
                var cultureCount = value.Cultures?.Count(c => c.PrintOnReport == "Yes") ?? 0;
                var specimenCommentCount = value.Comments?.Count(c => c.PrintOnReport == "Yes") ?? 0;
                _logWriter.LogInfo(
                    $"GetReportWithAmendments: specimenId={reportCriteria.Id}, directTestsOnReport={directTestCount}, " +
                    $"culturesOnReport={cultureCount}, specimenCommentsOnReport={specimenCommentCount} " +
                    $"(culture/isolate comments use persisted displayonreport, not amendment Comments list)",
                    nameof(ReportController),
                    nameof(GetReportWithAmendmentsAsync));
            }

            _specimenRecordReportHandler.ReportCriteria = reportCriteria.Value;
            var specimenRecord = await _specimenRecordReportHandler.HandleAsync(queryFilter, token);

            var result = await SerializeAndTranslateAsync(specimenRecord, token.LanguageId, _languageHandler);
            return Content(result ?? "{}", "application/json");
        }, nameof(GetReportWithAmendmentsAsync));
    }
}
