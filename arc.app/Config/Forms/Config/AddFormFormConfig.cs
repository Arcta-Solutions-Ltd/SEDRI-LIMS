using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddFormFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addformform',
                        viewTitle: 'Add new form.',
                        saveEvent: 'addform',
                        suppressRecordView: true,
                        pages: [ 'addformpage']
                    }";

            return form;
        }
    }
}
