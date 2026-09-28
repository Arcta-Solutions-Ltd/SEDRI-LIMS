using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteSourceFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletesourceform',
                        viewTitle: 'Delete a source.',
                        saveEvent: 'deletesource',
                        suppressRecordView: true,
                        pages: ['deletesourcepage']
                    }";

            return form;
        }
    }
}
