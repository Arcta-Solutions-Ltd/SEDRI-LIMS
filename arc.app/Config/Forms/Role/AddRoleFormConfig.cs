using arc.app.Common;

namespace arc.app.Config.Forms
{
    public class AddRoleFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'addroleform',
                    title: 'Adding a new role',
                    initialQuery: 'roleaddquery',
                    saveEvent: 'addrole',
                    suppressRecordView: true,
                    pages: [ 'roleadddetails', 'menupermission', 'eventpermission']
                }";

            return form;
        }
    }
}


