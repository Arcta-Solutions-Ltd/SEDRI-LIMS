using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Specimen Barcode list view.
    /// </summary>
    internal class BarcodeSpecimenListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Specimen Barcode list view.
        /// </summary>
        /// <returns>The list view configuration for the Specimen Barcode list view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'specimenbarcodes',
                            'type': 'ManageList',
                            'title': '@ManLabA@',
                            'headerText': '@LabTxtA@.',
                            'queryName': 'SpecimenBarcodeList',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@ConAlaA@', 'fieldName': 'Title', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true }
                            ],
                            'buttons': [
                                { 'key': 'editbarcode', 'text': '@CfgLabZ@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editspecimenbarcodeuievent', onFinish: 'refresh' }
                            ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
