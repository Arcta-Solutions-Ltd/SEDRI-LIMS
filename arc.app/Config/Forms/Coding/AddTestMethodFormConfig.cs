using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddTestMethodFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addtestmethodform',
                        viewTitle: 'Add a new test method.',
                        saveEvent: 'addtestmethod',
                        suppressRecordView: true,
                        pages: ['addtestmethodpage']
                    }";

            return form;
        }
    }
}
