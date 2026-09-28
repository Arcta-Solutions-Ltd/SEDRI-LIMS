using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class JsonViewerPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'jsonviewer',
                            pageTitle: '@MonVieC@', 
                            text: '@MonVieD@.',
                            crafted: true,                            
                            nextButton: { show: false },
                            cancelButton: { buttonText: '@GenClo@' }
                        }";

            return page;
        }
    }
}
