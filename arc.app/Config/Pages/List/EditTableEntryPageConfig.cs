using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditTableEntryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'edittableentrypage',
                            pageTitle: '@TabEdi@',
                            text: '@TabEdiB@.',
                            crafted: true
                        }";

            return page;
        }
    }
}
