using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class IqcTestProfileViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view =
                @"{
                    'name': 'iqctestprofile',
                    'type': 'ManageList',
                    'title': '@QuaIqcTesPro@',
                    'headerText': '@QuaIqcTesProDes@.',
                    'queryName': 'iqctestprofilelistquery',
                    'singleQuery': 'iqctestprofilesinglequery',
                    'gridColumns': [
                        { 'key': 'column1', 'name': '@GenOrgA@', 'fieldName': 'Organism', 'minWidth': 60, 'maxWidth': 160, 'isResizable': true },
                        { 'key': 'column2', 'name': '@ManQcStaBod@', 'fieldName': 'StandardsBody', 'minWidth': 60, 'maxWidth': 100, 'isResizable': true },
                        { 'key': 'column3', 'name': '@ManQcPriStr@', 'fieldName': 'PrimaryStrain', 'minWidth': 60, 'maxWidth': 100, 'isResizable': true },
                        { 'key': 'column4', 'name': '@ManQcOthStr@', 'fieldName': 'OtherStrains', 'minWidth': 120, 'maxWidth': 340, 'isResizable': true },
                        { 'key': 'column5', 'name': '@GenDef@', 'fieldName': 'UseByDefault', 'minWidth': 50, 'maxWidth': 50, 'isResizable': true },
                        { 'key': 'column6', 'name': '@GenAppA@', 'fieldName': 'Enabled', 'minWidth': 50, 'maxWidth': 50, 'isResizable': true }
                    ],
                    'buttons': [
                        { key: 'lists', text: '@QuaIqcTesPro@', icon: 'List',
                            buttons: [{key: 'addIqcTestProfile', text: '@QuaAddPro@', icon: 'AddToShoppingList', uievent: 'addiqctestprofileuievent', onFinish: 'refreshfilter' },
                                        {key: 'deleteIqcTestProfile', text: '@QuaDelPro@', icon: 'RemoveFromShoppingList', uievent: 'deleteiqctestprofileuievent', onFinish: 'refreshfilter' }]
                        },
                        { key: 'view', text: '@GenVieC@', icon: 'RedEye', onSelect: true, uievent: 'viewiqctestprofileqcorganismuievent' },
                        { key: 'edit', text: '@GenEdiB@', icon: 'Edit', onSelect: true, uievent: 'editiqctestprofileqcorganismuievent', onFinish: 'update' }
                    ],
                    filters: [
                        { key: 'iqctestprofiles', placeholder: '@QuaIqcTesProNam@', fieldName: 'iqctestprofileid', multiSelect: false, width: 160, optionsName: 'IqcTestProfiles', dynamic: true, includeFixed: true }
                    ],
                    filterPresets: [
                        { key: 'filter1', name: '@QuaMasDis@', default: true, fields: [ { key: 'iqctestprofiles', values: [ '1' ] } ] },
                        { key: 'filter2', name: '@QuaMasMic@', default: true, fields: [ { key: 'iqctestprofiles', values: [ '2' ] } ] }
                    ],
                    'filterSearch': true,
                    'searchFields': [ 'genusname', 'speciesname', 'standardsbody', 'primarystrain', 'otherstrains', 'subspeciesname', 'serotypename', 'additionalname' ]
                }";

            return JsonConvert.DeserializeObject<ListViewConfig>(view);
        }
    }
}
