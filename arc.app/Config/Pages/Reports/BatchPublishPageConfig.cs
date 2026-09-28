using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class BatchPublishPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'batchpublishpage',
                            pageTitle: '@RepBatA@',
                            text: '@RepPriC@.',
                            crafted: true,
                            nextButton: { show: false},
                            cancelButton: { buttonText: '@GenClo@' }
                        }";

            return page;
        }
    }
}
