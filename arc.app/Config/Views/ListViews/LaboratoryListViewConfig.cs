using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Configuration for the laboratory list view.
/// </summary>
internal class LaboratoryListViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the laboratory list view.
    /// </summary>
    /// <remarks>
    /// The configuration includes properties such as the view name, type, title, header text,
    /// query details, grid columns, buttons, and search settings.
    /// </remarks>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object representing the laboratory list view configuration.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                        'name': 'laboratories',
                        'type': 'ManageList',
                        'title': '@LabMan@',
                        'headerText': '@LabCre@.',
                        'queryName': 'LaboratoryList',
                        'singleQuery': 'SingleLaboratoryForLaboratoryList',
                        'gridColumns': [
                            { 'key': 'column1', 'name': '@LabLabA@', 'fieldName': 'LaboratoryName', 'minWidth': 100, 'maxWidth': 200, 'isResizable': true },
                            { key: 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                            { 'key': 'column2', 'name': '@GenLan@', 'fieldName': 'Language', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                            { 'key': 'column3', 'name': '@GenOrgF@', 'fieldName': 'CodingListId', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                            { 'key': 'column4', 'name': '@AntAnt@', 'fieldName': 'AntibioticGroupIds', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true }
                        ],
                        'buttons': [
                            { 'key': 'view', text: '@LabVie@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewlaboratoryrecorduievent' },
                            { 'key': 'turnaroundtime', text: '@GenTAT@', icon: 'Clock', onSelect: true, primaryAction: 5, uievent: 'turnaroundtimeuievent' },
                            { 'key': 'addlaboratory', 'text': '@LabAdd@', 'icon': 'Add', 'onSelect': false, uievent: 'addlaboratoryuievent', onFinish: 'refresh', primaryAction: 2 },
                            { 'key': 'editlaboratory', 'text': '@LabEdiA@', 'icon': 'Edit', 'onSelect': true, uievent: 'editlaboratoryuievent', onFinish: 'update', primaryAction: 3 },
                            { 'key': 'deletelaboratory', 'text': '@LabDelA@', 'icon': 'Delete', 'onSelect': true, uievent: 'deletelaboratoryuievent', onFinish: 'refresh', primaryAction: 4 }
                        ],
                        filterSearch: true,
                        searchFields: [ 'laboratoryname' ]
                    }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}