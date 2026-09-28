using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddSectionFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addsectionform',
                        viewTitle: 'Add section',
                        saveEvent: 'addsection',
                        suppressRecordView: true,
                        pages: [ 'addsectionpage', 'fieldselectorpage']
                    }";

            return form;
        }
    }
}
