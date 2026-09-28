using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteTestMethodFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletetestmethodform',
                        viewTitle: 'Delete a test method.',
                        saveEvent: 'deletetestmethod',
                        suppressRecordView: true,
                        pages: ['deletetestmethodpage']
                    }";

            return form;
        }
    }
}
