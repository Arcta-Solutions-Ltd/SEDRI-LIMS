using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class FieldSelectorPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'fieldselectorpage',
                            pageTitle: '@CodSelD@',
                            text: '@CodSelE@.',
                            crafted: true,
                            queryname: 'fieldselectionlistquery'
                        }";

            return page;
        }
    }
}
