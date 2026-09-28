using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews
{
    internal class RoleRecordViewConfig
    {
        internal RecordViewConfig GetView()
        {


            var view = @"{
                            'title': '@RolRolB@',
                            'name': 'roles',
                            'type': 'recordview',
                            'singleItemName': '@SinRol@',
                            'buttons': [
                                { 'key': 'editrole', 'text': '@RolEdiA@', 'icon': 'Edit', 'onSelect': true, primaryAction: 1, uievent: 'editrole' },
                                { 'key': 'menupermissions', 'text': '@RolMen@', 'icon': 'AllApps', 'onSelect': true, primaryAction: 2, uievent: 'menupermissions', onFinish: 'refresh' },
                                { 'key': 'eventpermissions', 'text': '@RolEve@', 'icon': 'Permissions', 'onSelect': true, primaryAction: 3, uievent: 'eventpermissions', onFinish: 'refresh' },
                                { 'key': 'clonerole', 'text': '@RolCloA@', 'icon': 'Share', 'onSelect': true, uievent: 'clonerole', onFinish: 'refresh' }
                            ],
                            'regions': [
                                { 'id': 'main',
                                  'type': 'standard',
                                  'queryName': 'rolebyidforrecordviewquery'
                                }
                            ]
                         }";

            var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

            return result;
        }
    }
}
