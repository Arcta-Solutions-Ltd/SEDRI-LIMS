using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DirectTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'directtestpage',
                            pageTitle: '@TesEntA@',
                            text: '@TesEntB@.',
                            crafted: true,
                            nextButton: { show: false },
                            cancelButton: { buttonText: '@GenClo@' }
                        }";

            return page;
        }
    }
}
