using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class RoleListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'roles',
                            'type': 'ManageList',
                            'title': '@RolMan@',
                            'headerText': '@RolCre@.',
                            'queryName': 'RoleList',
                            singleQuery: 'SingleRoleForRoleList',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@RolRolA@', 'fieldName': 'rolename', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@RolRol@', 'fieldName': 'roledescription', 'minWidth': 250, 'maxWidth': 500, 'isResizable': true },
                                { 'key': 'column4', 'name': '@GenEna@', 'fieldName': 'enabled', 'minWidth': 50, 'maxWidth': 100, 'isResizable': true }
                            ],
                            'buttons': [
                                { 'key': 'addrole', 'text': '@RolAdd@', 'icon': 'Add', 'onSelect': false, uievent: 'addrole', onFinish: 'refresh' },
                                { 'key': 'view', text: '@GenVieC@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewrolerecord' },
                                { 'key': 'editrole', 'text': '@RolEdiC@', 'icon': 'Edit', 'onSelect': true, primaryAction: 2, uievent: 'editrole', onFinish: 'update' },
                                { 'key': 'menupermissions', 'text': '@RolMen@', 'icon': 'AllApps', 'onSelect': true, uievent: 'menupermissions' },
                                { 'key': 'eventpermissions', 'text': '@RolEve@', 'icon': 'Permissions', 'onSelect': true, uievent: 'eventpermissions' },
                                { 'key': 'clonerole', 'text': '@RolCloA@', 'icon': 'Share', 'onSelect': true, uievent: 'clonerole', onFinish: 'refresh', refreshConfig: true },
                                { 'key': 'deleterole', 'text': '@RolDelA@', 'icon': 'Delete', 'onSelect': true, uievent: 'deleterole', onFinish: 'refresh', refreshConfig: true }
                            ],
                            filterSearch: true,
                            searchFields: [ 'rolename', 'roledescription' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
