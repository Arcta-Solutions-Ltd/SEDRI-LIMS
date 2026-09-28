using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class TableEntryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'tableentrypage',
                            pageTitle: '@TabAdd@',
                            text: '@TabAddA@.',
                            crafted: true,
                        }";

            return page;
        }
    }
}
