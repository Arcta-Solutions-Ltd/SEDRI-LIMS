using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddPageFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addpageform',
                        viewTitle: 'Add page definition.',
                        saveEvent: 'addpage',
                        initialquery: 'addpagequery',
                        suppressRecordView: true,
                        pages: [ 'addpagepage']
                    }";

            return form;
        }
    }
}
