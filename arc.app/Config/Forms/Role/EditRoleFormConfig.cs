using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditRoleFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editroleform',
                        viewTitle: 'General',
                        initialQuery: 'rolebyidforeditrole',
                        saveEvent: 'editrole',
                        recordView: 'roles',
                        pages: ['roleeditdetails']
                    }";

            return form;
        }
    }
}
