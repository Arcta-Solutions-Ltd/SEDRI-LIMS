using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Represents the configuration for the user list view in the application.
/// </summary>
internal class UserListViewConfig
{
    /// <summary>
    /// Retrieves the view configuration for displaying the user list.
    /// </summary>
    /// <returns>
    /// A <see cref="ListViewConfig"/> object containing the JSON-defined settings for
    /// the users list view, including grid columns, buttons, filters, and search fields.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = """
                    {
                        "name": "users",
                        "type": "ManageList",
                        "title": "@UseMan@",
                        "headerText": "@UseCre@.",
                        "queryName": "UserList",
                        "singleQuery": "SingleUserForUserList",
                        "gridColumns": [
                            {
                                "key": "column1",
                                "name": "@UseUseB@",
                                "fieldName": "Username",
                                "minWidth": 150,
                                "maxWidth": 150,
                                "isResizable": true
                            },
                            { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                            {
                                "key": "column2",
                                "name": "@PatFir@",
                                "fieldName": "FirstName",
                                "minWidth": 200,
                                "maxWidth": 200,
                                "isResizable": true
                            },
                            {
                                "key": "column3",
                                "name": "@UseLas@",
                                "fieldName": "LastName",
                                "minWidth": 200,
                                "maxWidth": 200,
                                "isResizable": true
                            },
                            {
                                "key": "column4",
                                "name": "@RolRolC@",
                                "fieldName": "Roles",
                                "minWidth": 200,
                                "maxWidth": 300,
                                "isResizable": true
                            },
                            {
                                "key": "column5",
                                "name": "@GenLab@",
                                "fieldName": "Laboratories",
                                "minWidth": 200,
                                "maxWidth": 300,
                                "isResizable": true
                            },
                            {
                                "key": "column6",
                                "name": "@GenOrg@",
                                "fieldName": "Organisations",
                                "minWidth": 200,
                                "maxWidth": 300,
                                "isResizable": true
                            },
                            {
                                "key": "column7",
                                "name": "@GenEna@",
                                "fieldName": "Enabled",
                                "minWidth": 100,
                                "maxWidth": 10,
                                "isResizable": true
                            }
                        ],
                        "buttons": [
                            {
                                "key": "adduser",
                                "text": "@UseAddA@",
                                "icon": "Add",
                                "onSelect": false,
                                "uievent": "adduser",
                                "onFinish": "refresh"
                            },
                            {
                                "key": "edituser",
                                "text": "@UseEdiA@",
                                "icon": "Edit",
                                "onSelect": true,
                                "primaryAction": 1,
                                "uievent": "edituser",
                                "onFinish": "update"
                            },
                            {
                                "key": "deleteuser",
                                "text": "@UseDel@",
                                "icon": "Delete",
                                "onSelect": true,
                                "primaryAction": 2,
                                "onFinish": "refresh",
                                "uievent": "deleteuseruievent"
                            },
                            {
                                "key": "changepassword",
                                "text": "@UseCha@",
                                "icon": "Lock12",
                                "onSelect": true,
                                "primaryAction": 3,
                                "uievent": "changepassworduievent"
                            }
                        ],
                        "filters": [
                            {
                                "key": "rolelist",
                                "placeholder": "@RolSel@",
                                "multiSelect": true,
                                "width": 160,
                                "optionsName": "RoleList",
                                "fieldName": "roleid"
                            }
                        ],
                        "filterSearch": true,
                        "searchFields": [
                            "username",
                            "firstname",

                    "lastname",
                    "organisation",
                    "role"

                        ]
                    }
                    """;
        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}