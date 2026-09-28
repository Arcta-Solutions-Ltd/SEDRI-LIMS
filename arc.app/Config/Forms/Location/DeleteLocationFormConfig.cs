using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteLocationFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletelocationform',
                        viewTitle: 'Delete an existing location.',
                        saveEvent: 'deletelocation',
                        initialQuery: 'locationbyid',
                        suppressRecordView: true,
                        pages: ['deletelocationpage']
                    }";

            return form;
        }
    }
}
