using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditPageFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editpageform',
                        viewTitle: 'Edit page definition.',
                        saveEvent: 'editpage',
                        initialquery: 'editpagequery',
                        suppressRecordView: true,
                        pages: [ 'editpagepage']
                    }";

            return form;
        }
    }
}
