using arc.app.Alert;
using arc.app.AST;
using arc.app.Config.Queries;
using arc.app.Reports;
using arc.app.Specimen;
using arc.app.Tests;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Reports;
using arc.domain.Tests;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public class SpecimenRecordUtils : ISpecimenRecordUtils
    {
        private readonly IQueryAdapter _queryAdapter;
        private readonly IConvertJsonStructureToKeyValuePair _pairConverter;
        private readonly IGenericRepository _genericRepository;
        private readonly ISpecimenRepository _specimenRepository;
        private readonly IAlertMessageHandler _alertMessageHandler;
        private readonly ITestRepository _testRepository;
        private readonly ITestForReport _testForReport;
        private readonly ICultureRepository _cultureRepository;
        private readonly IASTRepository _astRepository;
        private readonly IReplaceListItemValues _listReplacer;

        private List<string> _listOfListsUsedInReport = [];

        public SpecimenRecordUtils(IQueryAdapter queryAdapter, IConvertJsonStructureToKeyValuePair pairConverter, IGenericRepository genericRepository, ISpecimenRepository specimenRepository, IAlertMessageHandler alertMessageHandler,
            ITestRepository testRepository, ITestForReport testForReport, ICultureRepository cultureRepository, IASTRepository astRepository, IReplaceListItemValues listReplacer)
        {
            _queryAdapter = queryAdapter;
            _pairConverter = pairConverter;
            _genericRepository = genericRepository;
            _specimenRepository = specimenRepository;
            _alertMessageHandler = alertMessageHandler;
            _testRepository = testRepository;
            _testForReport = testForReport;
            _cultureRepository = cultureRepository;
            _astRepository = astRepository;
            _listReplacer = listReplacer;
        }

        public async Task<List<KeyValueModel>> GetPatientDetailsAsync(int patientId, TokenInfoModel token)
        {
            var queryData = await _queryAdapter.GetQueryAsync("PatientForPatientView");
            var queryFilters = new QueryFilterConfig
            {
                Name = "PatientForPatientView",
                Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "id", Value = patientId.ToString() } }
            };

            _genericRepository.AddConfiguration(queryData.TableName);
            _genericRepository.AddToken(token);
            var result = await _genericRepository.GetSingleAsync(queryData, queryFilters);

            return _pairConverter.Convert(result, true).ToList();
        }

        public async Task<List<KeyValueModel>> GetSpecimenDetailsAsync(string specimenId)
        {
            var queryData = await _queryAdapter.GetQueryAsync("SpecimenForSpecimenView");
            var parameters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "Id", Value = specimenId } } };
            var queryFilters = new QueryFilterConfig { Name = "SpecimenForSpecimenView", Parameters = parameters.Parameters };
            var result = await _genericRepository.GetSingleAsync(queryData, queryFilters);
            return _pairConverter.Convert(result, true).ToList();
        }

        public async Task<List<KeyValueModel>> GetApproverDetailsAsync(string specimenId)
        {
            var returnValue = new List<KeyValueModel>();
            var queryFilters = new QueryFilterConfig
            {
                Parameters = [new QueryValuesConfig { Key = "specimenid", Value = specimenId }]
            };

            var approvals = await _specimenRepository.SpecimenApprovalAsync(queryFilters);

            var keyValue = new KeyValueModel { Key = "SubmittedBy", Value = approvals.SubmittedBy };
            returnValue.Add(keyValue);
            keyValue = new KeyValueModel { Key = "SubmittedDate", Value = approvals.SubmittedDate.ToString() };
            returnValue.Add(keyValue);
            keyValue = new KeyValueModel { Key = "ApprovedBy", Value = approvals.ApprovedBy };
            returnValue.Add(keyValue);
            keyValue = new KeyValueModel { Key = "ApprovedDate", Value = approvals.ApprovedDate.ToString() };
            returnValue.Add(keyValue);

            return returnValue;
        }

        public async Task<List<TableRow>> GetAlertDetailsAsync(string specimenId)
        {
            var returnValue = new List<TableRow>();
            var queryFilters = new QueryFilterConfig
            {
                Parameters = new List<QueryValuesConfig> {
                new QueryValuesConfig { Key = "id", Value = specimenId },
                new QueryValuesConfig { Key = "view", Value = "specimens" },
            }
            };
            var alerts = await _alertMessageHandler.GetMessagesAsync(queryFilters);
            var topAlerts = new TableRow { Key = "topalerts", Rows = new List<string>() };
            var bottomAlerts = new TableRow { Key = "bottomalerts", Rows = new List<string>() };
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
            if (topAlerts.Rows.Count() > 0)
            {
                returnValue.Add(topAlerts);
            }
            if (bottomAlerts.Rows.Count() > 0)
            {
                returnValue.Add(bottomAlerts);
            }
            return returnValue;
        }

        public async Task<ReportData> GetDirectTestsAsync(string specimenId, ReportData returnValue, TokenInfoModel token)
        {
            var testList = await _testRepository.GetTestsForSpecimenAsync(int.Parse(specimenId));
            var reverseTestList = testList.ToList().Reverse<Test>();
            var savedTests = "";
            foreach (var test in reverseTestList)
            {
                if (test.Status == "Complete" && !savedTests.Contains("," + test.TestName.ToLower() + ","))
                {
                    returnValue = await _testForReport.LoadTestIntoReportAsync(test.TestName, test.Id, token, returnValue, null);
                    _listOfListsUsedInReport.AddRange(_testForReport.GetListFields());
                    savedTests += "," + test.TestName.ToLower() + ",";
                }
            }
            return returnValue;
        }

        public async Task<GroupRow> GetOrganismDataAsync(string specimenId, TokenInfoModel token)
        {
            var queryFilters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "specimenid", Value = specimenId } } };
            var cultures = await _cultureRepository.GetCultureListBySpecimenIdAsync(queryFilters);
            var groups = new GroupRow { Key = "Organisms", Multiple = new List<ReportData>() };
            var cultureListsUsed = new List<string>();
            foreach (var culture in cultures)
            {
                var newRow = new ReportData { Standard = new List<KeyValueModel>(), Tables = new List<TableRow>() };
                culture.OrganismWithGrowth = culture.BuildCultureReportHeading();
                var organismValues = _pairConverter.Convert(JsonConvert.SerializeObject(culture), true);
                newRow.Standard.AddRange(organismValues);

                queryFilters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "cultureid", Value = culture.Id } } };
                var cultureTestList = await _testRepository.GetTestsForCultureAsync(queryFilters);
                var savedTests = "";
                foreach (var test in cultureTestList)
                {
                    if (test.Status == "Complete" && !savedTests.Contains("," + test.TestName.ToLower() + ","))
                    {
                        newRow = await _testForReport.LoadTestIntoReportAsync(test.TestName, test.Id, token, newRow, null);
                        cultureListsUsed.AddRange(_testForReport.GetListFields());
                        savedTests += "," + test.TestName.ToLower() + ",";
                    }
                }

                queryFilters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "cultureid", Value = culture.Id } } };
                var astResults = await _astRepository.GetAstAntibioticListAsync(queryFilters);

                var lineText = "";
                var newRows = new List<string>();
                foreach (var astResult in astResults)
                {
                    lineText = astResult.Antibiotic + "|" + astResult.AntibioticCode + "|" + astResult.Susceptibility + "|" + astResult.TestMethodId + "|" + astResult.Dosage + "|" + astResult.GuidelinesId + "|" + astResult.Measurement;
                    newRows.Add(lineText);
                }

                if (newRows.Count > 0)
                {
                    newRow.Tables.Add(new TableRow { Key = "OrganismList", Rows = newRows });
                }

                newRow.Standard = await _listReplacer.ReplaceInStandardListAsync(newRow.Standard, cultureListsUsed);

                groups.Multiple.Add(newRow);
            }

            return groups;
        }

        public List<string> GetListsUsedInReport()
        {
            return _listOfListsUsedInReport;
        }
    }
}
