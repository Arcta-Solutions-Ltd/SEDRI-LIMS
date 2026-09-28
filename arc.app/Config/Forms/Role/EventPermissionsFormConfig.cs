using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EventPermissionsFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'eventpermissions',
                        viewTitle: 'Event Permissions',
                        initialQuery: 'roleeventpermissions',
                        saveEvent: 'eventPermissions',
                        recordView: 'roles',
                        suppressRecordView: false,
                        collapsible: true,
                        expanded: false,
                        pages: ['eventpermission']
                    }";

            return form;
        }
    }
}
