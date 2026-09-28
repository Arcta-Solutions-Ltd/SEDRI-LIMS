using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class CultureTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'culturetestpage',
                            pageTitle: '@TesEntP@',
                            text: '@TesEntQ@.',
                            crafted: true,
                            nextButton: { show: false },
                            cancelButton: { buttonText: '@GenClo@' }
                        }";

            return page;
        }
    }
}
