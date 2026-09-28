using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Represents the configuration for the Culture list view.
/// </summary>
internal class CultureListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Culture list view.
    /// </summary>
    /// <returns>The list view configuration for the Culture list view.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
                        'name': 'cultures',
                        'type': 'ManageList',
                        'title': '@SpeMan@',
                        'headerText': '@SpeCre@.',
                        'queryName': 'CultureListBySpecimenId',
                        'displaySummary': true,
                        'parentId': 'SpecimenId',
                        'displayifempty': false,
                        'groupby': 'ParentCultureId',
                        'grouptext': '@Type@ - @Growth@',
                        'groupMenu' : [ 
                            { key: 'addisolate', uievent: 'addisolateuievent', 'text': '@SpeAddN@', 'icon': 'Add', workflow: true }, 
                            { key: 'editculture', uievent: 'editcultureuievent', 'text': '@SpeEdiD@', 'icon': 'Edit', workflow: true },
                            { key: 'deleteculture', uievent: 'deletecultureuievent', 'text': '@SpeDelB@', 'icon': 'Delete', workflow: true }
                        ],
                        'gridColumns': [
                            { 'key': 'alert', name: '', fieldName: '', minWidth: 20, maxWidth: 20, isResizable: false, isCollapsible: false },
                            { 'key': 'column0', 'name': '@GenNum@', 'fieldName': 'CultureNumber', 'minWidth': 30, 'maxWidth': 30, 'isResizable': true, isCollapsible: false },
                            { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                            { 'key': 'column2', 'name': '@GenOrgA@', 'fieldName': 'SpecimenOrganism', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false },
                            { 'key': 'column3', 'name': '@GenQua@', 'fieldName': 'SpecimenQuantity', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: true }
                        ],
                        'buttons': [
                            { 'key': 'view', 'text': '@SpeVie@', 'icon': 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewculturerecord' },
                            { 'key': 'editisolate', 'text': '@SpeEdiA@', 'icon': 'Edit', onSelect: true, primaryAction: 2, uievent: 'editisolateuievent', workflow: true, onFinish: 'refresh' },
                            { 'key': 'ast', 'text': '@AstTes@', 'icon': 'TestBeakerSolid', onSelect: true,primaryAction: 3, 'uievent': 'ast', workflow: true, onFinish: 'refresh' },
                            { 'key': 'deleteisolate', 'text': '@SpeDel@', 'icon': 'Delete', onSelect: true, primaryAction: 4, 'uievent': 'deleteisolateuievent', workflow: true, onFinish: 'refresh' },
                            { 'key': 'editaliquot', 'text': '@SpeAliA@', 'icon': 'Edit', onSelect: true, primaryAction: 5, 'uievent': 'editaliquotuievent', workflow: true, onFinish: 'refresh' }  
                        ]
                    }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}

