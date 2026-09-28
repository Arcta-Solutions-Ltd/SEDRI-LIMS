using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Patient Barcode list view.
    /// </summary>
    internal class BarcodePatientListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Patient Barcode list view.
        /// </summary>
        /// <returns>The list view configuration for the Patient Barcode list view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'patientbarcodes',
                            'type': 'ManageList',
                            'title': '@ManLab@',
                            'headerText': '@LabTxt@.',
                            'queryName': 'PatientBarcodeList',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@ConAlaA@', 'fieldName': 'Title', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true }
                            ],
                            'buttons': [
                                { 'key': 'editbarcode', 'text': '@CfgLabZ@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editpatientbarcodeuievent', onFinish: 'refresh' }
                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
