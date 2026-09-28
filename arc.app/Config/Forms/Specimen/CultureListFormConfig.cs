using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class CultureListFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'culturelistform',
                            viewTitle: 'Cultures',
                            formtype: 'table',
                            collapsible: true,
                            expanded: true,
                            view: 'cultures'
                        }";

            return form;
        }
    }
}
