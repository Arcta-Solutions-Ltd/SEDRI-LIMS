using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class ChangePasswordFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'changepasswordform',
                        formtype: 'singlepage',
                        initialquery: 'userchangepassword',
                        saveEvent: 'changePassword',
                        suppressRecordView: true,
                        pages: ['changepasswordpage']
                    }";

            return form;
        }
    }
}
