using arc.app.Common;

namespace arc.app.Config.Forms
{
    public class DeleteRoleFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'deleteroleform',
                    title: 'Delete a role',
                    initialQuery: 'rolebyidforeditrole',
                    saveEvent: 'deleterole',
                    recordView: 'roles',
                    suppressRecordView: false,
                    pages: [ 'deleterolepage']
                }";

            return form;
        }
    }
}
