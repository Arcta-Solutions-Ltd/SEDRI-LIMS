using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddTagFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addtagform',
                        viewTitle: 'Add a new tag.',
                        saveEvent: 'addtag',
                        suppressRecordView: true,
                        pages: [ 'addtagpage']
                    }";

            return form;
        }
    }
}
