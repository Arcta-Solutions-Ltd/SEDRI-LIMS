using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class MenuPermissionsFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'menupermissions',
                        viewTitle: 'Menu Permissions',
                        initialQuery: 'rolemenupermissions',
                        saveEvent: 'menupermissions',
                        recordView: 'roles',
                        suppressRecordView: false,
                        collapsible: true,
                        expanded: true,
                        pages: ['menupermission']
                    }";

            return form;
        }
    }
}
