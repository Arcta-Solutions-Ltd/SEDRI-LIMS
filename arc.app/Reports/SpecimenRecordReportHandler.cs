using arc.app.Alert;
using arc.app.AST;
using arc.app.Common;
using arc.app.Config.Queries;
using arc.app.Configuration;
using arc.app.Specimen;
using arc.app.Tests;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.Reports;
using arc.common.Models.Specimen;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Reports;
using arc.domain.Tests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Assembles all data required to render a specimen record report.
/// Gathers patient demographics, specimen details, comments, alerts,
/// direct test results, and organism/culture data, applies report-status
/// colour mapping, and serialises the result to JSON for the PDF renderer.
/// </summary>
public class SpecimenRecordReportHandler : ISpecimenRecordReportHandler
{
    private readonly IQueryAdapter _queryAdapter;
    private readonly IGenericRepository _genericRepository;
    private readonly IConvertJsonStructureToKeyValuePair _pairConverter;
    private readonly ISpecimenRepository _specimenRepository;
    private readonly ICultureRepository _cultureRepository;
    private readonly ITestRepository _testRepository;
    private readonly IReplaceListItemValues _listReplacer;
    private readonly IASTRepository _astRepository;
    private readonly IAlertMessageHandler _alertMessageHandler;
    private readonly ITestForReport _testForReport;
    private readonly IReportStatusMapper _reportStatusMapper;
    private readonly ICommentRepository _commentRepository;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initialises a new instance of <see cref="SpecimenRecordReportHandler"/>.
    /// </summary>
    /// <param name="queryAdapter">Resolves named query definitions from configuration.</param>
    /// <param name="genericRepository">Executes general-purpose data queries against the database.</param>
    /// <param name="pairConverter">Converts JSON structures returned from queries into key-value pair lists consumed by the report renderer.</param>
    /// <param name="specimenRepository">Provides access to specimen records and approval data.</param>
    /// <param name="testRepository">Retrieves test records associated with a specimen or culture.</param>
    /// <param name="listReplacer">Replaces list-item numeric codes in standard field lists with their human-readable display values.</param>
    /// <param name="cultureRepository">Retrieves culture records linked to a specimen.</param>
    /// <param name="astRepository">Retrieves AST antibiotic results for a given culture.</param>
    /// <param name="alertMessageHandler">Fetches alert messages to be positioned at the top or bottom of the report.</param>
    /// <param name="testForReport">Loads a named test's data into the report data structure.</param>
    /// <param name="reportStatusMapper">Maps the raw specimen state string to a display-ready, colour-tagged value for the report heading.</param>
    /// <param name="commentRepository">Retrieves specimen and culture comments that are flagged for inclusion on the report.</param>
    /// <param name="logWriter">Writes diagnostic log entries for report assembly.</param>
    public SpecimenRecordReportHandler(IQueryAdapter queryAdapter, IGenericRepository genericRepository, IConvertJsonStructureToKeyValuePair pairConverter,
        ISpecimenRepository specimenRepository, ITestRepository testRepository, IReplaceListItemValues listReplacer, ICultureRepository cultureRepository, IASTRepository astRepository,
        IAlertMessageHandler alertMessageHandler, ITestForReport testForReport, IReportStatusMapper reportStatusMapper, ICommentRepository commentRepository, ILogWriter logWriter)
    {
        _queryAdapter = queryAdapter;
        _genericRepository = genericRepository;
        _pairConverter = pairConverter;
        _specimenRepository = specimenRepository;
        _testRepository = testRepository;
        _listReplacer = listReplacer;
        _cultureRepository = cultureRepository;
        _astRepository = astRepository;
        _alertMessageHandler = alertMessageHandler;
        _testForReport = testForReport;
        _reportStatusMapper = reportStatusMapper;
        _commentRepository = commentRepository;
        _logWriter = logWriter;
    }

        public SpecimenSelectorListModel ReportCriteria { get; set; }

        /// <summary>
        /// Builds the complete report data payload for a single specimen record and
        /// returns it as a serialised JSON string.
        /// <para>
        /// The following data is gathered in order:
        /// patient demographics, specimen details (with report-status colour mapping applied
        /// to the specimen state field), specimen comments, submitter/approver details,
        /// alert messages (top and bottom), direct test results, and organism/culture data
        /// including AST antibiotic results and culture comments.
        /// </para>
        /// </summary>
        /// <param name="parameters">
        /// Query filter containing at minimum an <c>id</c> integer parameter identifying
        /// the specimen to report on.
        /// </param>
        /// <param name="token">
        /// The authenticated user's token, forwarded to repository calls that require
        /// tenant or permission context.
        /// </param>
        /// <returns>
        /// A JSON string representing a <c>ReportData</c> object containing standard
        /// key-value fields, table rows (comments, alerts, AST results), and grouped
        /// organism rows ready for the PDF renderer.
        /// </returns>
        public async Task<string> HandleAsync(QueryFilterConfig parameters, TokenInfoModel token )
    {
        var stopwatch = Stopwatch.StartNew();
        var returnValue = new ReportData();
        var listOfListsUsedInReport = new List<string>();

        var specimenId = parameters.GetIntegerValue("id");
        _logWriter.LogInfo(
            $"SpecimenRecordReport started: specimenId={specimenId}",
            nameof(SpecimenRecordReportHandler),
            nameof(HandleAsync));

        var specimenPatient = await _specimenRepository.GetSingleAsync(specimenId);

        // Get Patient details

        var queryData = await _queryAdapter.GetQueryAsync("PatientForPatientView");
        var queryFilters = new QueryFilterConfig
        {
            Name = "PatientForPatientView",

            Parameters = [new() { Key = "id", Value = specimenPatient.PatientId.ToString() }]

        };

        _genericRepository.AddConfiguration(queryData.TableName);
        _genericRepository.AddToken(token);
        var result = await _genericRepository.GetSingleAsync(queryData, queryFilters);

        returnValue.Standard = _pairConverter.Convert(result, true).ToList();

        returnValue.Tables = [];

        // Get comments
        queryFilters = new QueryFilterConfig { Name = "reportcomments", Parameters = parameters.Parameters };
        result = await _commentRepository.GetReportCommentsAsync(queryFilters);

        var commentList = JsonConvert.DeserializeObject<List<CommentModel>>(result);

        var resultsToUse = commentList
            .Where(commentResult => commentResult.ShouldIncludeOnSpecimenReport(ReportCriteria))
            .ToList();

        _logWriter.LogInfo(
            $"Report comment inclusion: specimenId={specimenId}, reportCriteriaActive={ReportCriteria != null}, " +
            $"totalComments={commentList.Count}, includedComments={resultsToUse.Count}, " +
            $"cultureCommentsIncluded={resultsToUse.Count(c => c.IsCultureComment())}",
            nameof(SpecimenRecordReportHandler),
            nameof(HandleAsync));

        // Get Specimen Details
        queryData = await _queryAdapter.GetQueryAsync("SpecimenForSpecimenView");
        queryFilters = new QueryFilterConfig { Name = "SpecimenForSpecimenView", Parameters = parameters.Parameters };
        result = await _genericRepository.GetSingleAsync(queryData, queryFilters);
        var specimenValues = _pairConverter.Convert(result, true).ToList();


            var index = specimenValues.FindIndex(s => s.Key.Equals("state", StringComparison.CurrentCultureIgnoreCase));


        if (index != -1) specimenValues[index].Value = await _reportStatusMapper.AddMappingAsync(specimenValues[index].Value);

        returnValue.Standard.AddRange(specimenValues);

        var specimenCommentRows = resultsToUse
            .Where(comment => !comment.IsCultureComment())
            .Select(comment => comment.Comment)
            .ToList();

        returnValue.Tables.Add(new TableRow { Key = "SpecimenComments", Rows = specimenCommentRows });

        // Get specimen submitter & approver


        var approvals = await _specimenRepository.SpecimenApprovalAsync(new QueryFilterConfig().AddInteger("specimenId", specimenId));


        if (approvals.SubmittedDate.HasValue)
        {
            var keyValue = new KeyValueModel { Key = "SubmittedBy", Value = approvals.SubmittedBy };
            returnValue.Standard.Add(keyValue);
            var submittedDate = approvals.SubmittedDate ?? DateTime.Now;
            keyValue = new KeyValueModel { Key = "SubmittedDate", Value = submittedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") };
            returnValue.Standard.Add(keyValue);
        }

        if (approvals.ApprovedDate.HasValue)
        {
            var keyValue = new KeyValueModel { Key = "ApprovedBy", Value = approvals.ApprovedBy };
            returnValue.Standard.Add(keyValue);
            var approvedDate = approvals.ApprovedDate ?? DateTime.Now;
            keyValue = new KeyValueModel { Key = "ApprovedDate", Value = approvedDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") };
            returnValue.Standard.Add(keyValue);
        }


        //Need something to go to the database get the name and value for the image and add it to the key value pair list


        // Get Alerts

        var alerts = await _alertMessageHandler.GetMessagesAsync(
            new QueryFilterConfig().AddInteger("id", specimenId).AddString("view", "specimens"));
        var topAlerts = new TableRow { Key = "topalerts", Rows = [] };
        var bottomAlerts = new TableRow { Key = "bottomalerts", Rows = [] };

        foreach (var alert in alerts)
        {
            if (alert.ReportPositionId == 997)
            {
                bottomAlerts.Rows.Add(alert.Message);
            }
            else
            {
                if (alert.ReportPositionId == 998)
                {
                    topAlerts.Rows.Add(alert.Message);
                }
            }
        }
        if (topAlerts.Rows.Count > 0)
        {
            returnValue.Tables.Add(topAlerts);
        }
        if (bottomAlerts.Rows.Count > 0)
        {
            returnValue.Tables.Add(bottomAlerts);
        }

        // Get the direct tests
        var testList = await _testRepository.GetTestsForSpecimenAsync(specimenId);
        var reverseTestList = testList.ToList().Reverse<Test>();
        var savedTests = "";
        foreach (var test in reverseTestList)
        {
            if (test.Status == "Complete" && !savedTests.Contains("," + test.TestName.ToLower() + ","))
            {

                    returnValue = await _testForReport.LoadTestIntoReportAsync(test.TestName, test.Id, token, returnValue, ReportCriteria);

                listOfListsUsedInReport.AddRange(_testForReport.GetListFields());
                savedTests += "," + test.TestName.ToLower() + ",";
            }
        }

        // Get the organism data and culture comments


        var cultures = await _cultureRepository.GetCultureListBySpecimenIdAsync(new QueryFilterConfig().AddInteger("specimenid", specimenId));
        var groups = new GroupRow { Key = "Organisms", Multiple = [] };

        var cultureListsUsed = new List<string>();
        foreach (var culture in cultures)
        {

                if ((ReportCriteria == null && culture.DisplayOnReport == "Yes") || (ReportCriteria != null && ReportCriteria.IncludeCultureOnReport(culture.Id)))

            {
                var newRow = new ReportData { Standard = [], Tables = [] };
                culture.OrganismWithGrowth = culture.BuildCultureReportHeading();
                _logWriter.LogInfo(
                    $"Culture report heading: cultureId={culture.Id}, typeId={culture.TypeId}, growthId={culture.GrowthId}, quantityId={culture.SpecimenQuantityId}, displayHeading={culture.OrganismWithGrowth}",
                    nameof(SpecimenRecordReportHandler),
                    nameof(HandleAsync));
                var organismValues = _pairConverter.Convert(JsonConvert.SerializeObject(culture), true);
                newRow.Standard.AddRange(organismValues);

                queryFilters = new QueryFilterConfig { Parameters = [new() { Key = "cultureid", Value = culture.Id }] };
                var cultureTestList = await _testRepository.GetTestsForCultureAsync(queryFilters);
                savedTests = "";
                foreach (var test in cultureTestList)
                {
                    if (test.Status == "Complete" && !savedTests.Contains("," + test.TestName.ToLower() + ","))
                    {

                            newRow = await _testForReport.LoadTestIntoReportAsync(test.TestName, test.Id, token, newRow, null);

                        cultureListsUsed.AddRange(_testForReport.GetListFields());
                        savedTests += "," + test.TestName.ToLower() + ",";
                    }
                }

                var commentRows = resultsToUse
                    .Where(comment => comment.IsCultureComment() && comment.CultureId == culture.Id.ToString())
                    .Select(comment => comment.Comment)
                    .ToList();

                newRow.Tables.Add(new TableRow { Key = "CultureComments", Rows = commentRows });

                queryFilters = new QueryFilterConfig { Parameters = [new() { Key = "cultureid", Value = culture.Id }] };
                var astResults = await _astRepository.GetAstAntibioticListAsync(queryFilters);

                var buildResult = OrganismListReportRowBuilder.BuildRows(astResults);
                var newRows = buildResult.Rows;

                foreach (var astResult in buildResult.ExcludedRows)
                {
                    _logWriter.LogInfo(
                        $"AST report row excluded: cultureId={culture.Id}, astId={astResult.Id}, specialConsiderationId={astResult.SpecialConsiderationId}, parentDisplayOnReport={astResult.DisplayOnReport}, specialDisplayOnReport={astResult.SpecialDisplayOnReport}, effectiveDisplay={astResult.ResolveDisplayOnReportForReport()}",
                        nameof(SpecimenRecordReportHandler),
                        nameof(HandleAsync));
                }

                if (astResults.Count > 0)
                {
                    _logWriter.LogInfo(
                        $"AST report rows for cultureId={culture.Id}: total={astResults.Count}, included={buildResult.IncludedCount}, excluded={buildResult.ExcludedCount}, organListRowCount={newRows.Count}",
                        nameof(SpecimenRecordReportHandler),
                        nameof(HandleAsync));
                }
                if (newRows.Count > 0)
                {
                    newRow.Tables.Add(new TableRow { Key = "OrganismList", Rows = newRows });
                }

                newRow.Standard = await _listReplacer.ReplaceInStandardListAsync(newRow.Standard, cultureListsUsed);

                groups.Multiple.Add(newRow);
            }
        }


        returnValue.Groups = [groups];


        returnValue.Standard = await _listReplacer.ReplaceInStandardListAsync(returnValue.Standard, listOfListsUsedInReport);

        var cultureGroupCount = returnValue.Groups?.FirstOrDefault()?.Multiple?.Count ?? 0;
        stopwatch.Stop();
        _logWriter.LogInfo(
            $"SpecimenRecordReport completed: specimenId={specimenId}, standardFieldCount={returnValue.Standard.Count}, tableCount={returnValue.Tables.Count}, cultureGroupCount={cultureGroupCount}, elapsedMs={stopwatch.ElapsedMilliseconds}",
            nameof(SpecimenRecordReportHandler),
            nameof(HandleAsync));

        return JsonConvert.SerializeObject(returnValue);
    }
}

