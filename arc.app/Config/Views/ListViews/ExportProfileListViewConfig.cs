using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Represents the configuration for the Export Profile list view.
/// </summary>
internal class ExportProfileListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Export Profile list view.
    /// </summary>
    /// <returns>The list view configuration for the Export Profile list view.</returns>
    internal static ListViewConfig GetView()
    {
        var view = """
            {
                "name": "exportprofile",
                "type": "ManageList",
                "title": "@ExpPro@",
                "headerText": "@ExpProB@.",
                "singleQuery": "EditExportProfile",
                "queryName": "ExportProfileList",
                "gridColumns": [
                    {
                        "key": "column1",
                        "name": "@ExpProN@",
                        "fieldName": "name",
                        "minWidth": 200,
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
                        "name": "@ExpProD@",
                        "fieldName": "description",
                        "minWidth": 450,
                        "maxWidth": 450,
                        "isResizable": true
                    },
                    {
                        "key": "column3",
                        "name": "@ExpProMd@",
                        "fieldName": "modifieddate",
                        "minWidth": 150,
                        "maxWidth": 150,
                        "isResizable": true
                    }
                ],
                "buttons": [
                    {
                        "key": "addexportprofile",
                        "text": "@ExpProC@",
                        "icon": "Add",
                        "uievent": "addexportprofileuievent",
                        "onFinish": "refresh"
                    },
                    {
                        "key": "view",
                        "text": "@GenVieC@",
                        "icon": "RedEye",
                        "onSelect": true,
                        "primaryAction": 1,
                        "uievent": "viewexportprofilerecorduievent",
                        "onFinish": "refresh"
                    },
                    {
                        "key": "editexportprofile",
                        "text": "@GenEdiB@",
                        "icon": "Edit",
                        "uievent": "editexportprofileuievent",
                        "onFinish": "refresh",
                        "onSelect": true,
                        "primaryAction": 1
                    },
                    {
                        "key": "deleteexportprofile",
                        "text": "@GenDelC@",
                        "icon": "Delete",
                        "uievent": "deleteexportprofileuievent",
                        "onFinish": "refresh",
                        "onSelect": true,
                        "primaryAction": 2
                    },
                    {
                        "key": "manageexportprofilemapping",
                        "text": "@ExpProMap@",
                        "icon": "Mapping",
                        "uievent": "manageexportprofilemappinguievent",
                        "onFinish": "refresh",
                        "onSelect": true,
                        "primaryAction": 3
                    }
                ]
            }
            """;

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
