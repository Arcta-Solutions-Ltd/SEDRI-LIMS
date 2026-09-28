using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class TestPatternListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'testpatterns',
                            'type': 'ManageList',
                            'title': '@TesManA@',
                            'headerText': '@TesManB@.',
                            'queryName': 'TestPatternList',
                            'singleQuery': 'EditTestPattern',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'TestPatternName', 'minWidth': 140, 'maxWidth': 220, 'isResizable': true, isSorted: true, isSortedDescending: false },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 100, 'maxWidth': 100, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'column2', 'name': '@GenOrgA@', 'fieldName': 'OrganismName', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true },
                                { 'key': 'column3', 'name': '@GenOrd@', 'fieldName': 'OrderName', 'minWidth': 110, 'maxWidth': 110, 'isResizable': true },
                                { 'key': 'column4', 'name': '@GenFam@', 'fieldName': 'FamilyName', 'minWidth': 110, 'maxWidth': 110, 'isResizable': true },
                                { 'key': 'column5', 'name': '@GenOrgE@', 'fieldName': 'OrgGroupName', 'minWidth': 110, 'maxWidth': 500, 'isResizable': true },
                                { 'key': 'column6', 'name': '@SpeTyp@', 'fieldName': 'SpecimenTypes', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column7', 'name': '@GenHos@', 'fieldName': 'Host', 'minWidth': 60, 'maxWidth': 80, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column8', 'name': '@GenDef@', 'fieldName': 'Default', 'minWidth': 60, 'maxWidth': 80, 'isResizable': true }
                            ],
                            'buttons': [                                
                                { key: 'addTestPattern', 'text': '@TesAddK@', 'icon': 'Add', uievent: 'addtestpatternuievent', onFinish: 'refresh' },
                                { key: 'editTestPattern', text: '@TesEdiE@', icon: 'Edit', uievent: 'edittestpatternuievent', onFinish: 'refresh', onSelect: true, primaryAction: 1 },
                                { key: 'deleteTestPattern', text: '@TesDelD@', icon: 'Delete', uievent: 'deletetestpatternuievent', onFinish: 'refresh', onSelect: true, primaryAction: 2 }
                            ],
                            filters: [
                                { key: 'specimentype', fieldName: 'specimentypeid', placeholder: '@SpeTyp@', multiSelect: true, width: 160, optionsName: 'SpecimenType', dynamic: true, includeFixed: true },
                                { key: 'host', fieldName: 'hostid', placeholder: '@GenHos@', multiSelect: true, width: 160, optionsName: 'Host', dynamic: true, includeFixed: true }
                            ],
                            filterSearch: true,
                            searchFields: [ 'testpatternname' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}

                            //filterPresets: [
                            //    { key: 'filter1', name: '@OrgAll@', default: true, fields: [ { key: 'coding', values: [ '676'] } ] }
                            //],

                            //filterPresets: [
                            //    { key: 'filter1', name: '@CodCOM@', default: true, fields: [ { key: 'coding', values: [ '1000'] } ] }
                            //],
