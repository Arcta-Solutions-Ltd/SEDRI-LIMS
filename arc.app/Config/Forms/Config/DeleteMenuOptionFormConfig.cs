using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteMenuOptionFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletemenuoptionform',
                        viewTitle: 'Delete an existing menu option.',
                        saveEvent: 'deletemenuoption',
                        suppressRecordView: true,
                        pages: [ 'deletemenuoptionpage']
                    }";

            return form;
        }
    }
}
