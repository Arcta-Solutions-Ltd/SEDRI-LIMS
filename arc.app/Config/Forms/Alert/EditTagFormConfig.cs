using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditTagFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'edittagform',
                        viewTitle: 'Edit a tag.',
                        saveEvent: 'edittag',
                        initialQuery: 'singletagfortaglist',
                        pages: [ 'edittagpage']
                    }";

            return form;
        }
    }
}
