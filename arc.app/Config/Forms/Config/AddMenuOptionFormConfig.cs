using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddMenuOptionFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addmenuoptionform',
                        viewTitle: 'Add menu option.',
                        saveEvent: 'addmenuoption',
                        suppressRecordView: true,
                        pages: [ 'addmenuoptionpage']
                    }";

            return form;
        }
    }
}
