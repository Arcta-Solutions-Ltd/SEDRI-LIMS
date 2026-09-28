using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteTagFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletetagform',
                        viewTitle: 'Delete a tag.',
                        saveEvent: 'deletetag',
                        initialQuery: 'singletagfortaglist',
                        pages: [ 'deletetagpage']
                    }";

            return form;
        }
    }
}
