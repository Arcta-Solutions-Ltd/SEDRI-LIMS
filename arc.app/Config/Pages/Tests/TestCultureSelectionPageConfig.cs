using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class TestCultureSelectionPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'testcultureselectionpage',
                            pageTitle: '@TesManD@',
                            text: '@TesTesB@.',
                            crafted: true
                        }";

            return page;
        }
    }
}
