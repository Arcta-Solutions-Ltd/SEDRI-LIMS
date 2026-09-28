using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;
internal class WorkflowSpecimenTypeViewConfig
{
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'workflowspecimentypeview',
                            'type': 'ManageList',
                            'title': '@ConSpeA@',
                            'headerText': '@SpeCre@.',
                            'queryName': 'specimentypedirecttestmappinglistquery',
                            'displaySummary': false,
                            'addbutton': 'addspecimentypedirecttestuievent',
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@SpeSpeB@', 'fieldName': 'GroupDescription', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@ConDir@', 'fieldName': 'AssociatedList', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'editspecimentypeculturetype', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editspecimentypedirecttestuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'deletespecimentypeculturetype', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 1, uievent: 'deletespecimentypedirecttestuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
