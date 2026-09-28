using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class TestSelectionPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'testselectionpage',
                            pageTitle: '@TesMan@',
                            configureActions: 'nofields,delete',
                            tableName: 'None',
                            text: '@TesTes@.',
                            crafted: true
                        }";

            return page;
        }
    }
}
