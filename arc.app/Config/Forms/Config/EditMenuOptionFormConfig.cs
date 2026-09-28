using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditMenuOptionFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editmenuoptionform',
                        viewTitle: 'Edit an existing menu option.',
                        saveEvent: 'editmenuoption',
                        suppressRecordView: true,
                        pages: [ 'editmenuoptionpage']
                    }";

            return form;
        }
    }
}
