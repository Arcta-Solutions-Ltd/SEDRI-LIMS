using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Represents the configuration for the Export History list view.
/// </summary>
internal class ExportHistoryListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Export History list view.
    /// </summary>
    internal static ListViewConfig GetView()
    {
        var view = """
            {
                "name": "exporthistory",
                "type": "ManageList",
                "title": "@ExpExpHis@",
                "headerText": "@ExpExpHisB@",
                "singleQuery": "ExportHistoryRecordView",
                "queryName": "ExportHistoryList",
                "gridColumns": [
                    {
                        "key": "column1",
                        "name": "@ExpProN@",
                        "fieldName": "exportprofilename",
                        "minWidth": 200,
                        "maxWidth": 200,
                        "isResizable": true
                    },
                    {
                        "key": "columnSchedule",
                        "name": "@ExpSchNam@",
                        "fieldName": "schedulename",
                        "minWidth": 150,
                        "maxWidth": 200,
                        "isResizable": true
                    },
                    {
                        "key": "menu",
                        "name": "",
                        "fieldName": "",
                        "minWidth": 130,
                        "maxWidth": 130,
                        "isResizable": true,
                        "isCollapsible": false
                    },
                    {
                        "key": "column2",
                        "name": "@GenDat@",
                        "fieldName": "runat",
                        "minWidth": 180,
                        "maxWidth": 180,
                        "isResizable": true
                    }
                ],
                "buttons": [
                    {
                        "key": "view",
                        "text": "@GenVieC@",
                        "icon": "RedEye",
                        "onSelect": true,
                        "primaryAction": 1,
                        "uievent": "viewexporthistoryrecorduievent",
                        "onFinish": "refresh"
                    }
                ],
                "filters": [
                    {
                        "key": "ExportProfile",
                        "placeholder": "@ExpProN@",
                        "multiSelect": true,
                        "width": 180,
                        "optionsName": "exportprofilelist",
                        "fieldName": "exportprofileid",
                        "dynamic": true,
                        "includeFixed": true
                    }
                ],
                "filterSearch": true
            }
            """;

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
