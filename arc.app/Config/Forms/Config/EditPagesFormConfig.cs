using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditPagesFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editpagesform',
                        viewTitle: 'Edit pages definition.',
                        saveEvent: 'editpages',
                        initialquery: 'editpagesquery',
                        suppressRecordView: true,
                        pages: [ 'editpagespage']
                    }";

            return form;
        }
    }
}
