using arc.common.Models;
using arc.common.Models.Config;
using arc.common.Models.Laboratory;
using arc.domain.Configuration.BarcodeConfig;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.UIEventsConfig;
using arc.domain.Configuration.ViewConfig.DiaryViewConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using arc.domain.Configuration.ViewConfig.ReportingGridConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config;

public interface IConfigListUtils
{
    /// <summary>
    /// Asynchronously retrieves a list of form configurations based on the allowed event list.
    /// </summary>
    /// <param name="allowedEventList">A list of allowed event identifiers used to filter the form configurations.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="FormConfig"/>.
    /// </returns>
    Task<List<FormConfig>> GetFormConfigListAsync(List<string> allowedEventList);

    /// <summary>
    /// Asynchronously retrieves a list of page configurations.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="PageConfig"/>.
    /// </returns>
    Task<(List<PageConfig>, List<string>)> GetPagesAsync();

    /// <summary>
    /// Retrieves a list of UI event configurations.
    /// </summary>
    /// <returns>A list of <see cref="UIEventConfig"/>.</returns>
    List<UIEventConfig> GetUiEvents();

    /// <summary>
    /// Retrieves a list view configuration list based on the provided view and record list names.
    /// </summary>
    /// <param name="viewList">A list of view identifiers to filter the list views.</param>
    /// <param name="recordListNames">A list of record list names to include in the list views.</param>
    /// <returns>A list of <see cref="ListViewConfig"/>.</returns>
    List<ListViewConfig> GetListViewConfigList(List<string> viewList, List<string> recordListNames);

    /// <summary>
    /// Retrieves a list of record view configurations based on the provided list view configurations and record list names.
    /// </summary>
    /// <param name="listViewList">A list of list view configurations.</param>
    /// <param name="recordListNames">A list of record list names to include in the record views.</param>
    /// <returns>A list of <see cref="RecordViewConfig"/>.</returns>
    List<RecordViewConfig> GetRecordViewConfigList(List<ListViewConfig> listViewList, List<string> recordListNames);

    /// <summary>
    /// Asynchronously retrieves a list of barcode print configurations.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="BarcodePrintConfig"/>.
    /// </returns>
    Task<List<BarcodePrintConfig>> GetBarcodePrintConfigListAsync();

    /// <summary>
    /// Asynchronously retrieves a list of diary configurations.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="DiaryConfig"/>.
    /// </returns>
    Task<List<DiaryConfig>> GetDiaryConfigListAsync();

    /// <summary>
    /// Asynchronously retrieves a complete list of form configurations.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="FormConfig"/>.
    /// </returns>
    Task<List<FormConfig>> GetCompleteFormConfigListAsync();

    /// <summary>
    /// Removes add buttons from the provided list view configurations if permissions are not granted,
    /// based on the UI event configurations.
    /// </summary>
    /// <param name="listViews">A list of list view configurations.</param>
    /// <param name="uiEventList">A list of UI event configurations.</param>
    /// <returns>
    /// A list of <see cref="ListViewConfig"/> with add buttons removed where no permission is granted.
    /// </returns>
    List<ListViewConfig> RemoveAddButtonsFromViewsIfNoPermission(List<ListViewConfig> listViews, List<UIEventConfig> uiEventList);

    /// <summary>
    /// Retrieves a list of graph configurations based on the allowed sidebar item identifiers.
    /// </summary>
    /// <param name="allowedSideBarItems">An array of allowed sidebar item identifiers.</param>
    /// <returns>A list of <see cref="GraphConfig"/>.</returns>
    List<GraphConfig> GetGraphConfig(string[] allowedSideBarItems);

    /// <summary>
    /// Returns all analytics graph configurations for the home dashboard editor, regardless of sidebar access.
    /// </summary>
    /// <returns>Graph configs used to populate filter metadata and list dependencies for the home screen.</returns>
    List<GraphConfig> GetHomeDashboardGraphCatalog();

    /// <summary>
    /// Asynchronously retrieves a list of reporting grid configurations filtered by the specified view list.
    /// </summary>
    /// <param name="viewList">A list of view identifiers used to filter the reporting grid configurations.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="ReportingGridConfig"/>.
    /// </returns>
    Task<List<ReportingGridConfig>> GetReportingGridsAsync(List<string> viewList);

    /// <summary>
    /// Asynchronously retrieves the laboratory configuration based on the provided token information.
    /// </summary>
    /// <param name="token">The token containing laboratory and organization identification details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of 
    /// laboratory configuration models.
    /// </returns>
    Task<LaboratoryConfigurationListModel> GetLaboratoryConfigAsync(TokenInfoModel token);

    /// <summary>
    /// Retrieves and maps workflow configuration models from the configuration cache.
    /// </summary>
    /// <returns>
    /// A list of <see cref="WorkflowConfigModel"/> instances representing the workflow configurations.
    /// </returns>
    List<WorkflowConfigModel> GetWorkflowConfigList();

    Task<List<FormConfig>> GetAdditonalFormConfigListAsync(List<string> uiEvents, List<string> allowedEventList);
}
