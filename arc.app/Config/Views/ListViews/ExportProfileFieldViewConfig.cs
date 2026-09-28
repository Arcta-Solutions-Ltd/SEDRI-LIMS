using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Export Profile Field list view.
    /// </summary>
    internal class ExportProfileFieldViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Export Profile Field list view.
        /// </summary>
        /// <returns>The list view configuration for the Export Profile Field view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'exportprofilefield',
                            'type': 'ManageList',
                            'title': '@ExpProRecFieldsTitle@',
                            'headerText': '@ExpProRecFieldsTitle@.',
                            'queryName': 'exportprofilerecordview',
                            'parentId': 'ExportProfileId',
                            'addbutton': 'addexportprofilefielduievent',

                            'DisplayIfEmpty':true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@ExpProFielHead@', 'fieldName': 'HeaderName', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@ExpProRecTbl@', 'fieldName': 'TableName', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true },
                                { 'key': 'column3', 'name': '@MapTit@', 'fieldName': 'Mapping', 'minWidth': 200, 'maxWidth': 200, 'isResizable': true },
                            ],
                            'buttons': [
                                { key: 'deleteExportProfileField', text: '@ExpProRecFieldDel@', icon: 'Delete', uievent: 'deleteexportprofilefielduievent', onSelect: true, primaryAction: 1, onFinish: 'refresh' }
                                 ]
                            
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
