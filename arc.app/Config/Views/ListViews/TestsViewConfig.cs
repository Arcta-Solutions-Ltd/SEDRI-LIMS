using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class TestsViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view =
                @"{
                    'name': 'tests',
                    'type': 'DirectTestsList',
                    'title': '@ConDirA@',
                    'headerText': '@SpeManB@.',
                    'queryName': 'activetestlistquery',
                    'singleQuery': 'activetestbyidfortestlistquery',
                    'displaysummary': true,
                    'gridColumns': [
                        { Key: 'alert', Name: '', FieldName: '', MinWidth: 20, MaxWidth: 20, IsResizable: false, IsCollapsible: false },
                        { key: 'turnaroundtime', name: '', fieldName: 'TurnAroundTimeColour', minWidth: 28, maxWidth: 28, isResizable: false },
                        { key: 'column1', name: '@SpeAcc@', fieldName: 'AccessionNumber', minWidth: 135, maxWidth: 135, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
                        { Key: 'column2', Name: '@GenTesC@', FieldName: 'TestDescription', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: false },
                        { Key: 'menu', Name: '', FieldName: '', MinWidth: 120, MaxWidth: 120, IsResizable: true, IsCollapsible: false },
                        { Key: 'column3', Name: '@GenStaA@', FieldName: 'Status', MinWidth: 200, MaxWidth: 200, IsResizable: true, IsCollapsible: true },
                        { key: 'column4', name: '@PatFir@', fieldName: 'FirstName', minWidth: 90, maxWidth: 90, isResizable: true, isCollapsible: false },
                        { key: 'column5', name: '@PatSurA@', fieldName: 'Surname', minWidth: 90, maxWidth: 90, isResizable: true, isCollapsible: false },
                        { key: 'column6', name: '@PatRef@', fieldName: 'PatientRef', minWidth: 90, maxWidth: 90, isResizable: true, isCollapsible: false },
                        { key: 'column7', name: '@SpeSpeB@', fieldName: 'SpecimenType', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                        { Key: 'column8', Name: '@GenReq@', FieldName: 'Requested', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: true },
                        { Key: 'column9', Name: '@GenComC@', FieldName: 'Completed', MinWidth: 200, MaxWidth: 200, IsResizable: true, IsCollapsible: true }
                    ],
                    'buttons': [   
                        { 'key': 'view', 'text': '@GenVieC@', 'icon': 'RedEye', 'onSelect': true, uievent: 'viewtestrecord', onFinish: 'update', primaryAction: 1 },
                        { 'key': 'edittest', 'text': '@GenEdi@', 'icon': 'Edit', 'onSelect': true, onFinish: 'update', primaryAction: 2 },
                        { 'key': 'deletetest', 'text': '@GenDel@', 'icon': 'Delete', 'onSelect': true, uievent: 'removedirecttestuievent', onFinish: 'refresh', primaryAction: 3 }
                    ],
                    filters: [
                        { key: 'teststatus', placeholder: '@GenTesF@', multiSelect: false, width: 140, optionsName: 'teststatus', fieldName: 'teststatus' },
                        { key: 'testtype', placeholder: '@GenTesD@', multiSelect: true, width: 200, optionsName: 'directtestconfiglist', fieldName: 'testtype' },
                        { key: 'specimentype', placeholder: '@GenTyp@', multiSelect: true, width: 240, optionsName: 'SpecimenType', fieldName: 'specimentypeid' }
                    ],
                    'filterSearch': true,
                    'searchFields': [ 'accessionnumber', 'firstname', 'surname', 'patientref', 'barcode', 'existingbarcode', 'status', 'specimentype' ]
                }";

            return JsonConvert.DeserializeObject<ListViewConfig>(view);
        }
    }
}
