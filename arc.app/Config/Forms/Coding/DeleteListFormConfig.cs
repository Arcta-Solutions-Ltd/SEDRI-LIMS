using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteListFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletelistform',
                        viewTitle: 'Delete a list.',
                        saveEvent: 'deletecodinglist',
                        suppressRecordView: true,
                        pages: ['deletelistpage']
                    }";

            return form;
        }
    }
}
