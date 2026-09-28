using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditTestMethodFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'edittestmethodform',
                        viewTitle: 'Edit a test method.',
                        saveEvent: 'edittestmethod',
                        suppressRecordView: true,
                        pages: ['edittestmethodpage']
                    }";

            return form;
        }
    }
}
