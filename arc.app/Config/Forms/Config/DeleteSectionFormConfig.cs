using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteSectionFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletesectionform',
                        viewTitle: 'Delete section',
                        saveEvent: 'deletesection',
                        suppressRecordView: true,
                        initialQuery: 'deletesectionquery',
                        pages: [ 'deletesectionpage']
                    }";

            return form;
        }
    }
}
