using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;
internal class SpecimenTypeWorkflowMappingViewConfig
{
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'specimentypeworkflowmappingview',
                            'type': 'ManageList',
                            'title': '@LabCulA@',
                            'headerText': '@LabManD@.',
                            'queryName': 'specimentypeworkflowlistquery',
                            'displaySummary': false,
                            'addbutton': 'addspecimentypeworkflowuievent',
                            'addButtonTooltip': '@LabAddJ@',
                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenWorA@', 'fieldName': 'GroupDescription', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@LabSpeB@', 'fieldName': 'AssociatedList', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'edit', 'text': '@GenEdiB@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editspecimentypeworkflowuievent', onFinish: 'embeddedrefresh' },
                                { 'key': 'delete', 'text': '@GenDelC@', 'icon': 'Delete', 'onSelect': true, primaryAction: 1, uievent: 'deletespecimentypeworkflowuievent', onFinish: 'embeddedrefresh' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
