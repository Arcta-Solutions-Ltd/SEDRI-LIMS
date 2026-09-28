using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditUserFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'edituserform',
                        formtype: 'singlepage',
                        initialQuery: 'userbyid',
                        saveEvent: 'edituser',
                        suppressRecordView: true,
                        pages: ['usereditdetails', 'userproperties']
                    }";

            return form;
        }
    }
}
