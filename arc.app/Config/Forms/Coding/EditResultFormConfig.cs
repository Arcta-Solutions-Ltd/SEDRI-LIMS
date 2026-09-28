using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditResultFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editresultform',
                        viewTitle: 'Edit an existing result.',
                        saveEvent: 'editresult',
                        suppressRecordView: true,
                        pages: ['editresultpage']
                    }";

            return form;
        }
    }
}
