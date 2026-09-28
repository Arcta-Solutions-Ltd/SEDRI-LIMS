using arc.app.Common;
using arc.app.Config.Graphs;
using arc.app.Config.Pages;
using arc.app.Config.UIEvents;
using arc.app.Config.Validation;
using arc.app.Config.Views.DiaryViews;
using arc.app.Config.Views.ReportingGrid;
using arc.app.Configuration;
using arc.app.Laboratory;
using arc.common.Models;
using arc.common.Models.Config;
using arc.common.Models.Laboratory;
using arc.common.Utils;
using arc.domain.Configuration.BarcodeConfig;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.UIEventsConfig;
using arc.domain.Configuration.ViewConfig.DiaryViewConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using arc.domain.Configuration.ViewConfig.ReportingGridConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Config;

/// <summary>
/// Provides utility methods for handling various configuration lists, including list view, record view, UI events, diaries, forms, barcode prints, graphs, and reporting grids.
/// </summary>
/// <param name="listViewConfigFactory">Factory for creating list view configurations.</param>
/// <param name="recordViewConfigFactory">Factory for creating record view configurations.</param>
/// <param name="eventConfigAdapter">Adapter for retrieving event configurations.</param>
/// <param name="diaryFactory">Factory for creating diary configurations.</param>
/// <param name="formConfigList">List of form configurations.</param>
/// <param name="barcodePrintConfigAdapter">Adapter for retrieving barcode print configurations.</param>
/// <param name="graphConfigFactory">Factory for creating graph configurations.</param>
/// <param name="reportingGridFactory">Factory for creating reporting grid configurations.</param>
public class ConfigListUtils(IListViewConfigFactory listViewConfigFactory, IRecordViewConfigFactory recordViewConfigFactory, IUIEventConfigAdapter eventConfigAdapter,
    IDiaryFactory diaryFactory, IFormConfigList formConfigList, IBarcodePrintConfigAdapter barcodePrintConfigAdapter, IGraphConfigFactory graphConfigFactory,

    IReportingGridFactory reportingGridFactory, IGeneralRepository generalRepository, IListRepository listRepository, IConfigCache configCache,
    ILaboratoryConfigurationHandler laboratoryConfigurationHandler, IListViewTestsValidator listViewTestsValidator) : IConfigListUtils

{
    private readonly IListViewConfigFactory _listViewConfigFactory = listViewConfigFactory;
    private readonly IReportingGridFactory _reportingGridFactory = reportingGridFactory;
    private readonly IRecordViewConfigFactory _recordViewConfigFactory = recordViewConfigFactory;
    private readonly IUIEventConfigAdapter _eventConfigAdapter = eventConfigAdapter;
    private readonly IDiaryFactory _diaryFactory = diaryFactory;
    private readonly IBarcodePrintConfigAdapter _barcodePrintConfigAdapter = barcodePrintConfigAdapter;
    private readonly IFormConfigList _formConfigList = formConfigList;
    private readonly IGraphConfigFactory _graphConfigFactory = graphConfigFactory;
    private readonly IGeneralRepository _generalRepository = generalRepository;
    private readonly IListRepository _listRepository = listRepository;
    private readonly IConfigCache _configCache = configCache;
    private readonly ILaboratoryConfigurationHandler _laboratoryConfigurationHandler = laboratoryConfigurationHandler;
    private readonly IListViewTestsValidator _listViewTestsValidator = listViewTestsValidator;

    private List<string> _uiEventList = [];

    /// <summary>
    /// Asynchronously retrieves the list of form configurations based on the provided allowed event list.
    /// </summary>
    /// <param name="allowedEventList">The list of allowed event names.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of form configurations.</returns>
    public async Task<List<FormConfig>> GetFormConfigListAsync(List<string> allowedEventList)
    {
            await _formConfigList.LoadFormsAsync(_uiEventList,allowedEventList);
            return _formConfigList.GetForms();
        }

        /// <summary>
        /// Asynchronously retrieves the list of form configurations based on the provided allowed event list.
        /// </summary>
        /// <param name="allowedEventList">The list of allowed event names.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of form configurations.</returns>
        public async Task<List<FormConfig>> GetAdditonalFormConfigListAsync(List<string> uiEvents, List<string> allowedEventList)
        {
            await _formConfigList.LoadFormsAsync(uiEvents, allowedEventList);

        return _formConfigList.GetForms();
    }

    /// <summary>
    /// Asynchronously retrieves the complete list of form configurations.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the complete list of form configurations.</returns>
    public async Task<List<FormConfig>> GetCompleteFormConfigListAsync()
    {
        return await _formConfigList.GetCompleteFormListAsync(_uiEventList);
    }

    /// <summary>
    /// Asynchronously retrieves the list of page configurations.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of page configurations.</returns>
    public async Task<(List<PageConfig>, List<string>)> GetPagesAsync()
    {
            var (pageList, uiEventList) = await _formConfigList.GetPagesAsync();
            _uiEventList.AddRange(uiEventList);
            return (pageList, uiEventList);
    }

    /// <summary>
    /// Gets the list of UI events to display.
    /// </summary>
    /// <returns>A list of UIEventConfig objects representing the UI events to display.</returns>
    public List<UIEventConfig> GetUiEvents()
    {
        return _formConfigList.GetUiEvents();
    }

    /// <summary>
    /// Asynchronously retrieves the reporting grid configurations for the specified view list.
    /// </summary>
    /// <param name="viewList">The list of view names to retrieve reporting grid configurations for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of reporting grid configurations.</returns>
    public async Task<List<ReportingGridConfig>> GetReportingGridsAsync(List<string> viewList)
    {
        var reportingGridsToDisplay = new List<ReportingGridConfig>();

        foreach (var view in viewList)
        {
            var newConfig = await _reportingGridFactory.GetViewAsync(view);
            if (newConfig != null)
            {
                reportingGridsToDisplay.Add(newConfig);
            }
        }

        return reportingGridsToDisplay;
    }

    /// <summary>
    /// Retrieves a list of list view configurations based on the provided view and record list names.
    /// </summary>
    /// <param name="viewList">The list of view names to retrieve configurations for.</param>
    /// <param name="recordListNames">The list of record list names to consider for embedded list views.</param>
    /// <returns>A list of <see cref="ListViewConfig"/> objects representing the list views to display.</returns>
    public List<ListViewConfig> GetListViewConfigList(List<string> viewList, List<string> recordListNames)
    {
        var listViewsToDisplay = new List<ListViewConfig>();

        // Get top-level list-views.
        foreach (var view in viewList)
        {
            var newListViewConfig = _listViewConfigFactory.GetViewAsync(view).Result;
            if (newListViewConfig != null)
            {
                _listViewTestsValidator.SanitizeViewTestsAsync(newListViewConfig).GetAwaiter().GetResult();
                listViewsToDisplay.Add(newListViewConfig);

                var listOfEvents = newListViewConfig.GetUIEventList();
                _uiEventList.AddRange(listOfEvents);
            }
        }

        // Find and add list-views embedded at multiple levels (max 3 currently).
        for (int level = 0; level < 2; level++)
        {
            var embeddedRecords = GetRecordViewConfigList(listViewsToDisplay, recordListNames);
            foreach (var recordView in embeddedRecords)
            {
                foreach (var region in recordView.Regions)
                {
                    var duplicate = false;
                    if (region.Type == "listview" || region.Name is "isolatelist")
                    {
                        foreach (var listView in listViewsToDisplay)
                        {
                            if (listView.Name == region.ListViewName)
                            {
                                duplicate = true;
                            }
                        }

                        var newListViewConfig = _listViewConfigFactory.GetViewAsync(region.ListViewName).Result;
                        if (newListViewConfig != null && !duplicate)
                        {
                            _listViewTestsValidator.SanitizeViewTestsAsync(newListViewConfig).GetAwaiter().GetResult();
                            listViewsToDisplay.Add(newListViewConfig);

                            var listOfEvents = newListViewConfig.GetUIEventList();
                            _uiEventList.AddRange(listOfEvents);
                        }
                    }
                }
            }
        }

        // Deal with UI Events that must be added manually

        _uiEventList.Add("culturetestuievent");
        _uiEventList.Add("culturecommentuievent");
        _uiEventList.Add("managecultureattachmentsuievent");
        _uiEventList.Add("specimenprintpreviewuievent");

        // Reference forms for barcode label field labels (PrintBarcode / BarcodeItem GetFieldsForForm).
        if ((viewList.Contains("specimens") || viewList.Contains("patients")) && !_uiEventList.Contains("createspecimenreceived"))
        {
            _uiEventList.Add("createspecimenreceived");
        }
        if (viewList.Contains("patients") && !_uiEventList.Contains("createspecimenreceivedforpatientuievent"))
        {
            _uiEventList.Add("createspecimenreceivedforpatientuievent");
        }

        if (_uiEventList.Contains("culturetestuievent") && !_uiEventList.Contains("culturetestselectionuievent"))
        {
            _uiEventList.Add("culturetestselectionuievent");
        }

        if (viewList.Contains("specimens"))
        {
            _uiEventList.Add("addspecimentaguievent");
            _uiEventList.Add("managespecimenattachmentsuievent");
        }

        if (viewList.Contains("patients"))
        {
            _uiEventList.Add("addpatienttaguievent");
            _uiEventList.Add("batchaddpatienttaguievent");
            _uiEventList.Add("managepatientattachmentsuievent");
            _uiEventList.Add("manageadmissionattachmentsuievent");
            _uiEventList.Add("managerequestattachmentsuievent");
        }

        if (_uiEventList.Contains("pageconfiguievent"))
        {
            _uiEventList.Add("addfielduievent");
            _uiEventList.Add("addexistingfielduievent");
            _uiEventList.Add("addformgroupuievent");
            _uiEventList.Add("deletefielduievent");
            _uiEventList.Add("deleteformgroupuievent");
            _uiEventList.Add("editfielduievent");
            _uiEventList.Add("editformgroupuievent");
            _uiEventList.Add("movefielduievent");
            _uiEventList.Add("moveformgroupuievent");
            _uiEventList.Add("reorderformgroupsuievent");
            _uiEventList.Add("addreportconfiguievent");
            _uiEventList.Add("addpageuievent");
            _uiEventList.Add("editpagesuievent");
            _uiEventList.Add("editpageuievent");
            _uiEventList.Add("editpagerulesuievent");
            _uiEventList.Add("deletepageuievent");
        }

        if (_uiEventList.Contains("reportconfiguievent"))
        {
            _uiEventList.Add("addsectionuievent");
            _uiEventList.Add("deletesectionuievent");
            _uiEventList.Add("editsectionuievent");
            _uiEventList.Add("editreportsectionuievent");
        }
        if (_uiEventList.Contains("addexportprofilefielduievent"))
        {
            _uiEventList.Add("editexportprofilefieldsuievent");
        }
        if (_uiEventList.Contains("addexportscheduleuievent"))
        {
            _uiEventList.Add("editexportscheduleuievent");
            _uiEventList.Add("deleteexportscheduleuievent");
        }
        _uiEventList.Add("runexportuievent");
        _uiEventList.Add("mypassworduievent");
        _uiEventList.Add("preferenceuievent");
        _uiEventList.Add("addstateuievent");
        _uiEventList.Add("editstateuievent");
        _uiEventList.Add("deletestateuievent");
        _uiEventList.Add("cultureprintselectoruievent");
        _uiEventList.Add("removedirecttestuievent");
        _uiEventList.Add("removeculturetestuievent");

        _uiEventList.Add("editiqcresultuievent");
        _uiEventList.Add("deleteiqcresultuievent");
        _uiEventList.Add("editiqctestqcorganismsuievent");
        _uiEventList.Add("editaccessionnumberuievent");

        _uiEventList.Add("editpatientreferenceuievent");
        _uiEventList.Add("editcommentforselectoruievent");

        _uiEventList.Add("ordercommentsuievent");

        return listViewsToDisplay.OrderBy(l => l.Name).ToList();
    }

    /// <summary>
    /// Retrieves a list of record view configurations based on the provided list view configurations and record list names.
    /// </summary>
    /// <param name="listViewList">The list of list view configurations to use for retrieving record views.</param>
    /// <param name="recordListNames">The list of record list names to retrieve configurations for.</param>
    /// <returns>A list of <see cref="RecordViewConfig"/> objects representing the record views to display.</returns>
    public List<RecordViewConfig> GetRecordViewConfigList(List<ListViewConfig> listViewList, List<string> recordListNames)
    {
        var recordViewsToDisplay = new List<RecordViewConfig>();

        // Add record views based on list views
        foreach (var listView in listViewList)
        {

            var recordViewName = string.IsNullOrEmpty(listView.RecordView) ? listView.Name : listView.RecordView;
            var newRecordViewConfig = _recordViewConfigFactory.GetViewAsync(recordViewName).Result;

            if (newRecordViewConfig != null)
            {
                recordViewsToDisplay.Add(newRecordViewConfig);
            }
        }

        // Add record views based on provided record list names
        foreach (var recordView in recordListNames)
        {

            var newRecordViewConfig = _recordViewConfigFactory.GetViewAsync(recordView).Result;

            if (newRecordViewConfig != null)
            {
                recordViewsToDisplay.Add(newRecordViewConfig);
            }
        }

        return recordViewsToDisplay;
    }

    /// <summary>
    /// Asynchronously retrieves a list of barcode print configurations based on UI events.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of barcode print configurations.</returns>
    public async Task<List<BarcodePrintConfig>> GetBarcodePrintConfigListAsync()
    {
        var barcodePrintConfigsToDisplay = new List<BarcodePrintConfig>();

        foreach (var uiEvent in _uiEventList)
        {
            var newEvent = await _eventConfigAdapter.GetEventAsync(uiEvent);
            if (newEvent?.Type.Equals("print-barcode", StringComparison.OrdinalIgnoreCase) == true)
            {
                var newBarcodePrintConfig = await _barcodePrintConfigAdapter.GetBarcodePrintConfigAsync(newEvent.Action);
                if (newBarcodePrintConfig != null)
                {
                    barcodePrintConfigsToDisplay.Add(newBarcodePrintConfig);
                }
            }
        }

        return barcodePrintConfigsToDisplay;
    }

    /// <summary>
    /// Asynchronously retrieves a list of diary configurations based on UI events.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of diary configurations.</returns>
    public async Task<List<DiaryConfig>> GetDiaryConfigListAsync()
    {
        var diaryToDisplay = new List<DiaryConfig>();
        var tempEventList = new List<string>();

        foreach (var uiEvent in _uiEventList)
        {
            var newEvent = await _eventConfigAdapter.GetEventAsync(uiEvent);
            if (newEvent?.Type.Equals("diary", StringComparison.OrdinalIgnoreCase) == true)
            {
                var newDiaryConfig = _diaryFactory.Create(newEvent.Action);
                if (newDiaryConfig != null)
                {
                    diaryToDisplay.Add(newDiaryConfig);
                    var listOfEvents = newDiaryConfig.GetUIEventList();
                    tempEventList.AddRange(listOfEvents);
                }
            }
        }

        _uiEventList.AddRange(tempEventList);

        return diaryToDisplay;
    }

    /// <summary>
    /// Removes the Add button from views if the corresponding UI event is not present in the allowed event list.
    /// </summary>
    /// <param name="listViews">The list of list view configurations.</param>
    /// <param name="uiEventList">The list of allowed UI event configurations.</param>
    /// <returns>A list of <see cref="ListViewConfig"/> objects with the Add button removed if not allowed.</returns>
    public List<ListViewConfig> RemoveAddButtonsFromViewsIfNoPermission(List<ListViewConfig> listViews, List<UIEventConfig> uiEventList)
    {
        foreach (var view in listViews)
        {
            if (!string.IsNullOrWhiteSpace(view.AddButton))
            {
                var ev = uiEventList.FirstOrDefault(e => e.Name.Equals(view.AddButton, StringComparison.OrdinalIgnoreCase));
                if (ev == null)
                {
                    view.AddButton = null;
                }
            }
        }

        return listViews;
    }

    /// <summary>
    /// Retrieves the graph configurations based on the allowed sidebar items.
    /// </summary>
    /// <param name="allowedSideBarItems">The array of allowed sidebar items.</param>
    /// <returns>A list of <see cref="GraphConfig"/> objects representing the graph configurations to display.</returns>
    public List<GraphConfig> GetGraphConfig(string[] allowedSideBarItems)
    {
        var reportList = new[]
        {
            "specimentypesummarygraph",
            "specimenstategraph",
            "locationgraph",
            "taggraph",
            "organisationgraph",
            "organismgraph",
            "organismsusceptibilitygraph",
            "gendersummarygraph",
            "testgraph"
        };
        var returnList = new List<GraphConfig>();

        if (allowedSideBarItems.Contains("graphs"))
        {
            foreach (var report in reportList)
            {
                var graphConfig = _graphConfigFactory.GetGraph(report);
                if (graphConfig != null)
                {
                    returnList.Add(graphConfig);
                }
            }
        }

        return returnList;

    }

    /// <summary>
    /// Returns graph definitions for the home dashboard editor (all analytics graph types), independent of sidebar <c>graphs</c> permission.
    /// Used so list options for filters load even when the user does not have Analytics in the menu.
    /// </summary>
    /// <returns>One <see cref="GraphConfig"/> per known analytics graph name.</returns>
    public List<GraphConfig> GetHomeDashboardGraphCatalog()
    {
        var reportList = new[]
        {
            "specimentypesummarygraph",
            "specimenstategraph",
            "locationgraph",
            "taggraph",
            "organisationgraph",
            "organismgraph",
            "organismsusceptibilitygraph",
            "gendersummarygraph",
            "testgraph"
        };
        var returnList = new List<GraphConfig>();
        foreach (var report in reportList)
        {
            var graphConfig = _graphConfigFactory.GetGraph(report);
            if (graphConfig != null)
            {
                returnList.Add(graphConfig);
            }
        }

        return returnList;
    }


    /// <summary>
    /// Asynchronously retrieves laboratory configuration settings using the provided token information.
    /// </summary>
    /// <param name="token">
    /// The token model containing the laboratory and organisation identifiers as string values.
    /// </param>
    /// <returns>
    /// An anonymous object containing the configuration list, the parsed laboratory identifier, and the parsed organisation identifier.
    /// </returns>
    public async Task<LaboratoryConfigurationListModel> GetLaboratoryConfigAsync(TokenInfoModel token)
    {
        //if (int.TryParse(token.LaboratoryId, out int laboratoryId) && laboratoryId > 0)
        //{
        //    await _laboratoryConfigurationHandler.LoadSingleLaboratoryConfigurationAsync(laboratoryId);
        //}
        //else
        //{
            await _laboratoryConfigurationHandler.LoadConfigurationForAllLaboratoriesAsync();
        //}
        return _laboratoryConfigurationHandler.LaboratoryConfigurationList;
    }

    /// <summary>
    /// Retrieves and maps workflow configuration models from the configuration cache.
    /// </summary>
    /// <returns>
    /// A list of <see cref="WorkflowConfigModel"/> instances representing the workflow configurations.
    /// </returns>
    public List<WorkflowConfigModel> GetWorkflowConfigList()
    {
        return _configCache.GetConfigList(18)
                           .Select(workflowConfig =>
                           {
                               var newRecord = ArcJson.Deserialize<WorkflowConfigModel>(workflowConfig.Contents);
                               newRecord.Id = workflowConfig.Id;
                               return newRecord;
                           })
                           .ToList();
    }
}
