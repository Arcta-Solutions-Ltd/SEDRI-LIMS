using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Configuration class for the replay instrument error page.
    /// </summary>
    internal class ReplayInstrumentErrorPageConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the replay instrument error page.
        /// </summary>
        /// <returns>A JSON string that represents the page configuration.</returns>
        public string Get()
        {
            var page = @"{
                            name: 'replayinstrumenterrorpage',
                            pageTitle: '@InsRep@',
                            text: '@InsRepB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Message', type: 'json', label: '@GenMes@'},
                                                { id: 'ErrorText', type: 'multitext', label: '@GenErr@'}
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
