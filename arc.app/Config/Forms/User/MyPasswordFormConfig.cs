using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class MyPasswordFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'mypasswordform',
                        formtype: 'singlepage',
                        saveEvent: 'myPassword',
                        suppressRecordView: true,
                        pages: ['mypasswordpage']
                    }";

            return form;
        }
    }
}
