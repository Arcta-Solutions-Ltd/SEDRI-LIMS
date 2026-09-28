using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditWordFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editwordform',
                        viewTitle: 'Edit language entry.',
                        saveEvent: 'editword',
                        suppressRecordView: true,
                        useListData: true,
                        pages: ['editwordpage']
                    }";

            return form;
        }
    }
}
