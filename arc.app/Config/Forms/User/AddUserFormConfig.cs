using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddUserFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'adduserform',
                        formtype: 'singlepage',
                        saveEvent: 'addUser',
                        suppressRecordView: true,
                        pages: ['useradddetails', 'userproperties']
                    }";

            return form;
        }
    }
}
