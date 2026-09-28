using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Alert Type list view.
    /// </summary>
    internal class AlertTypeViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Alert Type list view.
        /// </summary>
        /// <returns>The list view configuration for the Alert Type list view.</returns>
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'alerttype',
                            'type': 'ManageList',
                            'title': '@AleManB@',
                            'headerText': '@AleCreA@.',
                            'queryName': 'AlertCategoryList',
                            'singleQuery': 'SingleForAlertCategoryList',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@AleAle@', 'fieldName': 'name', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'column2', 'name': '@AleCat@', 'fieldName': 'alertcategory', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'column3', 'name': '@AlePos@', 'fieldName': 'position', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'column4', 'name': '@RepPos@', 'fieldName': 'reportposition', 'minWidth': 200, 'maxWidth': 400, 'isResizable': true }
                            ],
                            'buttons': [
                                { key: 'addAlertCategory', 'text': '@AleAddC@', 'icon': 'Add', uievent: 'addalertcategoryuievent', onFinish: 'refresh' },
                                { key: 'editAlertCategory', text: '@AleEdiE@', icon: 'Edit', uievent: 'editalertcategoryuievent', onSelect: true, onFinish: 'update' },
                                { key: 'deleteAlertCategory', text: '@AleDelD@', icon: 'Remove', uievent: 'deletealertcategoryuievent', onSelect: true, onFinish: 'refresh' }
                            ],
                            filters: [
                                { key: 'alertcategory', placeholder: '@AleCat@', multiSelect: true, width: 140, optionsName: 'AlertType', fieldName: 'alertcategoryid' },
                                { key: 'position', placeholder: '@GenPos@', multiSelect: true, width: 200, optionsName: 'AlertPosition', fieldName: 'positionid' },
                                { key: 'reportposition', placeholder: '@AleDis@', multiSelect: true, width: 200, optionsName: 'AlertPosition', fieldName: 'reportpositionid' }
                            ],
                            filterSearch: true,
                            searchFields: [ 'name', 'alertcategory', 'position', 'reportposition' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
