using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Configuration class for the batch replay instrument error page.
    /// </summary>
    internal class BatchReplayInstrumentErrorPageConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the batch replay instrument error page.
        /// </summary>
        /// <returns>A JSON string that represents the page configuration.</returns>
        public string Get()
        {
            var page = @"{
                            name: 'batchreplayinstrumenterrorpage',
                            pageTitle: '@InsBatB@',
                            text: '@InsBatC@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenRepC@' }
                        }";

            return page;
        }
    }
}
