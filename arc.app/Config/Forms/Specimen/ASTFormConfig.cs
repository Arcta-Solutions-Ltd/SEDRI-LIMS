using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class ASTFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'ast',
                            initialQuery: 'astaddquery',
                            saveEvent: 'updateast',
                            formtype: 'singlepage',
                            suppressRecordView: true,
                            pages: ['ast']
                         }";

            return form;
        }
    }
}
