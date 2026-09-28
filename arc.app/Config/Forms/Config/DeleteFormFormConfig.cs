using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteFormFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleteformform',
                        viewTitle: 'Delete an existing form.',
                        saveEvent: 'deleteform',
                        suppressRecordView: true,
                        pages: [ 'deleteformpage']
                    }";

            return form;
        }
    }
}
