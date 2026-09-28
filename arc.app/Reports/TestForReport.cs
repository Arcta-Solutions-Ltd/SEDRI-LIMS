using arc.app.Common;
using arc.app.Config.Forms;
using arc.app.Config.Pages;
using arc.app.Config.Queries;
using arc.app.Config.Reports.DataSection;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.Reports;
using arc.common.Utils;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Reports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Reports
{
    /// <summary>
    /// Class responsible for handling test report functionalities.
    /// </summary>
    public class TestForReport : ITestForReport
    {
        private IFormConfigAdapter _formAdapter;
        private readonly IQueryAdapter _queryAdapter;
        private readonly IHandleQuery _queryHandler;
        private readonly IConvertJsonStructureToKeyValuePair _pairConverter;
        private readonly IConvertJsonStructureToPipeSeparatedList _pipeConverter;
        private readonly IReplaceListItemValues _listReplacer;
        private readonly IDataSectionAdapter _dataSectionAdapter;
        private readonly IPageConfigAdapter _pageConfigAdapter;
        private readonly IJsonElementRemover _jsonRemover;
        private readonly ILogWriter _logWriter;

        private List<FieldConfig> _listFields = new List<FieldConfig>();
        private List<FieldConfig> _listOfGrids = new List<FieldConfig>();

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        /// <param name="formAdapter">Form configuration adapter.</param>
        /// <param name="queryAdapter">Query adapter.</param>
        /// <param name="queryHandler">Query handler.</param>
        /// <param name="pairConverter">Converter for JSON structure to key-value pairs.</param>
        /// <param name="pipeConverter">Converter for JSON structure to pipe-separated list.</param>
        /// <param name="listReplacer">Service to replace list item values.</param>
        /// <param name="dataSectionAdapter">Data section adapter.</param>
        /// <param name="jsonRemover">JSON element remover.</param>
        /// <param name="pageConfigAdapter">Page configuration adapter.</param>
        /// <param name="logWriter">Log writer used to record how each test grid was matched to a data section grid.</param>
        public TestForReport(IFormConfigAdapter formAdapter, IQueryAdapter queryAdapter, IHandleQuery queryHandler, IConvertJsonStructureToKeyValuePair pairConverter, IConvertJsonStructureToPipeSeparatedList pipeConverter,
            IReplaceListItemValues listReplacer, IDataSectionAdapter dataSectionAdapter, IJsonElementRemover jsonRemover, IPageConfigAdapter pageConfigAdapter, ILogWriter logWriter)
        {
            _formAdapter = formAdapter;
            _queryAdapter = queryAdapter;
            _queryHandler = queryHandler;
            _pairConverter = pairConverter;
            _pipeConverter = pipeConverter;
            _listReplacer = listReplacer;
            _dataSectionAdapter = dataSectionAdapter;
            _pageConfigAdapter = pageConfigAdapter;
            _jsonRemover = jsonRemover;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Loads test data into the report asynchronously.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <param name="testId">The ID of the test.</param>
        /// <param name="token">Token information model.</param>
        /// <param name="returnValue">The current report data to be returned.</param>
        /// <param name="reportCriteria">The criteria for the specimen selector list model.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the updated report data.</returns>
        public async Task<ReportData> LoadTestIntoReportAsync(string testName, int testId, TokenInfoModel token, ReportData returnValue, SpecimenSelectorListModel reportCriteria)
        {
            var testForm = await _formAdapter.GetFormAsync(testName);
            var dataSection = await _dataSectionAdapter.GetSectionAsync(testForm.DataSection);
            await GetFieldsForFormAsync(testForm);

            var queryData = await _queryAdapter.GetQueryAsync(testForm.InitialQuery);
            var queryFilters = new QueryFilterConfig
            {
                Name = testForm.InitialQuery,
                Parameters = [new QueryValuesConfig { Key = "id", Value = testId.ToString() }]
            };

            var result = await _queryHandler.HandleAsync(testForm.InitialQuery, queryFilters, queryData, token);
            var display = JsonConvert.DeserializeObject<PrintOnReportModel>(result);
            if ((reportCriteria == null && display.PrintOnReport == "Yes") || (reportCriteria != null && reportCriteria.IncludeDirectTestOnReport(testId.ToString())))
            {
                result = _jsonRemover.RemoveElementsByValue(result, "printonreport");
                var testValues = _pairConverter.Convert(result, true);

                if (dataSection.Grids.Count > 0)
                {
                    var pipeValues = _pipeConverter.Convert(result);
                    var gridTables = await BuildGridTablesAsync(testForm, testName, dataSection, pipeValues);

                    foreach (var gridTable in gridTables)
                    {
                        AddRowsToTable(returnValue, gridTable);
                    }
                }

                returnValue.Standard.AddRange(testValues);
            }

            return returnValue;
        }

        /// <summary>
        /// Gathers a test's grid rows under the data keys the report renders, one entry per data
        /// section grid the test supplied data for.
        /// </summary>
        /// <param name="testForm">The test's form configuration, used to work out which grid columns hold list items.</param>
        /// <param name="testName">The name of the test, used in log messages.</param>
        /// <param name="dataSection">The data section the test's form points at, which declares the available grids and their data keys.</param>
        /// <param name="pipeValues">The test's grid arrays, each labelled with the form fieldgrid id it came from.</param>
        /// <returns>
        /// A task whose result is one entry per data key. Rows are written as entered where a key has a
        /// single source grid; where a data section declares fewer grids than the form has fieldgrids the
        /// sources share a key and each row is prefixed with its source grid's label so the report can
        /// split them into headed sub tables.
        /// </returns>
        private async Task<List<GridTableContentsModel>> BuildGridTablesAsync(FormConfig testForm, string testName, DataSectionConfig dataSection, List<GridLineContentsModel> pipeValues)
        {
            var fallbackKey = dataSection.Grids.First().Data;
            var sources = new List<(string DataKey, string GridId, string Label, List<string> Rows)>();

            foreach (var grid in pipeValues)
            {
                var listToChange = await IdentifyWhichColumnsAreListsAsync(testForm, grid.Contents, grid.Label);
                var rows = await _listReplacer.ReplaceInGridAsync(grid.Contents.ToList(), listToChange);

                // The grid is matched on its id, not on the label or description, because those are
                // language catalogue tokens that change with the user's language.
                var dataGrid = dataSection.Grids.FirstOrDefault(g => g.Name.IsSameGridId(grid.Label));
                var dataKey = dataGrid?.Data ?? fallbackKey;

                if (dataGrid == null)
                {
                    _logWriter.LogInfo($"Test {testName} formDataSection={testForm.DataSection} grid '{grid.Label}' is not declared by data section, adding its {rows.Count} row(s) to data key '{fallbackKey}'", nameof(TestForReport), nameof(BuildGridTablesAsync));
                }
                else
                {
                    _logWriter.LogInfo($"Test {testName} formDataSection={testForm.DataSection} grid '{grid.Label}' matched data section grid '{dataGrid.Name}', {rows.Count} row(s) under data key '{dataKey}'", nameof(TestForReport), nameof(BuildGridTablesAsync));
                }

                var gridField = _listOfGrids.FirstOrDefault(g => g.Id.IsSameGridId(grid.Label));
                sources.Add((dataKey, grid.Label, gridField?.Label ?? grid.Label, rows.ToList()));
            }

            var tables = new List<GridTableContentsModel>();

            foreach (var group in sources.GroupBy(s => s.DataKey, StringComparer.OrdinalIgnoreCase))
            {
                var groupedSources = group.ToList();
                var keyIsShared = groupedSources.Count > 1;

                if (keyIsShared)
                {
                    _logWriter.LogInfo($"Test {testName} data key '{group.Key}' is shared by {groupedSources.Count} grids, prefixing each row with its grid name", nameof(TestForReport), nameof(BuildGridTablesAsync));
                }

                var table = new GridTableContentsModel { DataKey = group.Key };

                foreach (var source in groupedSources)
                {
                    table.SourceGridIds.Add(source.GridId);
                    table.Rows.AddRange(keyIsShared
                        ? source.Rows.Select(r => source.Label + "|" + r)
                        : source.Rows);
                }

                tables.Add(table);
            }

            return tables;
        }

        /// <summary>
        /// Adds a gathered grid table to the report data, appending to an existing table where an
        /// earlier test already contributed rows under the same data key.
        /// </summary>
        /// <param name="reportData">The report data being built.</param>
        /// <param name="gridTable">The rows to add and the data key they belong to.</param>
        private void AddRowsToTable(ReportData reportData, GridTableContentsModel gridTable)
        {
            var existing = reportData.Tables.FirstOrDefault(t => string.Equals(t.Key, gridTable.DataKey, StringComparison.OrdinalIgnoreCase));

            if (existing == null)
            {
                reportData.Tables.Add(new TableRow { Key = gridTable.DataKey, Rows = gridTable.Rows });
                return;
            }

            existing.Rows ??= new List<string>();
            existing.Rows.AddRange(gridTable.Rows);
        }

        /// <summary>
        /// Gets a list of field names.
        /// </summary>
        /// <returns>A list of field names as strings.</returns>
        public List<string> GetListFields()
        {
            return _listFields.Select(x => x.Id).ToList();
        }

        /// <summary>
        /// Retrieves fields for the given form asynchronously.
        /// </summary>
        /// <param name="form">The form configuration.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task GetFieldsForFormAsync(FormConfig form)
        {
            _listFields.Clear();
            // Cleared alongside the fields: the handler reuses one instance across every test on a
            // specimen, so grids left behind from an earlier test would be matched against this one.
            _listOfGrids.Clear();
            foreach (var page in form.Pages)
            {
                var pageConfig = await _pageConfigAdapter.GetPageAsync(page);
                _listFields.AddRange(pageConfig.GetFieldList("dropdown"));
                _listFields.AddRange(pageConfig.GetFieldList("combobox"));
                _listFields.AddRange(pageConfig.GetFieldList("hierarchicalpicker"));
                _listOfGrids.AddRange(pageConfig.GetFieldList("fieldgrid"));
            }
        }

        /// <summary>
        /// Identifies which columns are lists in a grid, asynchronously.
        /// </summary>
        /// <param name="form">The form configuration.</param>
        /// <param name="contents">The contents of the grid.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains a list of booleans indicating whether each column is a list.</returns>
        private async Task<List<bool>> IdentifyWhichColumnsAreListsAsync(FormConfig form, List<string> contents, string fieldName)
        {
            var listOfGridFields = new List<FieldGridConfig>();

            foreach (var page in form.Pages)
            {
                var pageConfig = await _pageConfigAdapter.GetPageAsync(page);
                listOfGridFields.AddRange(pageConfig.GetGridFieldList(fieldName));
            }

            var listToChange = listOfGridFields.Select(f => (f.Type.ToLower() == "dropdown" || f.Type.ToLower() == "combobox" || f.Type.ToLower() == "hierarchicalpicker"));

            return listToChange.ToList();
        }
    }
}
