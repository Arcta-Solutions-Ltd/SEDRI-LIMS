using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteAlertFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletealertform',
                        viewTitle: 'Delete an existing alert.',
                        saveEvent: 'deletealert',
                        initialQuery: 'editalert',
                        pages: ['deletealertpage']
                    }";

            return form;
        }
    }
}
