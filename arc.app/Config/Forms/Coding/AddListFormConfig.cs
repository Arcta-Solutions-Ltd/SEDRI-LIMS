using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddListFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addlistform',
                        viewTitle: 'Add a new list.',
                        saveEvent: 'addcodinglist',
                        suppressRecordView: true,
                        pages: ['addlistpage']
                    }";

            return form;
        }
    }
}
