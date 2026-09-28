using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddResultFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addresultform',
                        viewTitle: 'Add a new result.',
                        saveEvent: 'addresult',
                        suppressRecordView: true,
                        pages: ['addresultpage']
                    }";

            return form;
        }
    }
}
