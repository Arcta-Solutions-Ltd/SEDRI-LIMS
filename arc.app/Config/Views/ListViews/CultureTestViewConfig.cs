using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Culture Test list view.
    /// </summary>
    internal class CultureTestViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Culture Test list view.
        /// </summary>
        /// <returns>The list view configuration for the Culture Test list view.</returns>
        internal static ListViewConfig GetView()
        {
            var view =
                @"{
                    'name': 'culturetests',
                    'type': 'DirectTestsList',
                    'title': '@SpeCulC@',
                    'headerText': '@TesManC@.',
                    'queryName': 'activeculturetestlistquery',
                    'singleQuery': 'activeculturetestbyidfortestlistquery',
                    'displaysummary': true,
                    'gridColumns': [
                        { Key: 'alert', Name: '', FieldName: '', MinWidth: 20, MaxWidth: 20, IsResizable: false, IsCollapsible: false },
                        { key: 'turnaroundtime', name: '', fieldName: 'TurnAroundTimeColour', minWidth: 28, maxWidth: 28, isResizable: false },
                        { key: 'column1', name: '@SpeAcc@', fieldName: 'AccessionNumber', minWidth: 135, maxWidth: 135, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
                        { Key: 'column2', Name: '@GenTesC@', FieldName: 'TestDescription', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: false },
                        { Key: 'menu', Name: '', FieldName: '', MinWidth: 120, MaxWidth: 120, IsResizable: true, IsCollapsible: false },
                        { Key: 'column3', Name: '@GenStaA@', FieldName: 'Status', MinWidth: 200, MaxWidth: 200, IsResizable: true, IsCollapsible: true },
                        { Key: 'column4', Name: '@CulTyp@', FieldName: 'CultureType', MinWidth: 200, MaxWidth: 200, IsResizable: true, IsCollapsible: true },
                        { key: 'column5', name: '@PatFir@', fieldName: 'FirstName', minWidth: 90, maxWidth: 90, isResizable: true, isCollapsible: false },
                        { key: 'column6', name: '@PatSurA@', fieldName: 'Surname', minWidth: 90, maxWidth: 90, isResizable: true, isCollapsible: false },
                        { key: 'column7', name: '@PatRef@', fieldName: 'PatientRef', minWidth: 90, maxWidth: 90, isResizable: true, isCollapsible: false },
                        { key: 'column8', name: '@SpeSpeB@', fieldName: 'SpecimenType', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                        { Key: 'column9', Name: '@GenReq@', FieldName: 'Requested', MinWidth: 180, MaxWidth: 180, IsResizable: true, IsCollapsible: true },
                        { Key: 'column10', Name: '@GenComC@', FieldName: 'Completed', MinWidth: 200, MaxWidth: 200, IsResizable: true, IsCollapsible: true }
                    ],
                    'buttons': [
                        { 'key': 'edittest', 'text': '@GenEdi@', 'icon': 'Edit', 'onSelect': true, onFinish: 'update', primaryAction: 1 }
                    ],
                    filters: [
                        { key: 'teststatus', placeholder: '@GenTesF@', multiSelect: false, width: 140, optionsName: 'teststatus', fieldName: 'teststatus' },
                        { key: 'testtype', placeholder: '@GenTesD@', multiSelect: true, width: 200, optionsName: 'culturetestconfiglist', fieldName: 'testtype' },
                        { key: 'specimentype', placeholder: '@GenTyp@', multiSelect: true, width: 240, optionsName: 'SpecimenType', fieldName: 'specimentypeid' },
                        { key: 'culturetype', placeholder: '@CulTypA@', multiSelect: true, width: 240, optionsName: 'CultureType', fieldName: 'culturetypeid' }
                    ],
                    'filterSearch': true,
                    'searchFields': [ 'accessionnumber', 'firstname', 'surname', 'patientref', 'barcode', 'existingbarcode', 'status', 'specimentype', 'culturetype' ]
                }";

            return JsonConvert.DeserializeObject<ListViewConfig>(view);
        }
    }
}
