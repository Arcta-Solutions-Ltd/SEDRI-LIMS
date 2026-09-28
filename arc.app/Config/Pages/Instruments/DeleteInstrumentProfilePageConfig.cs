using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Configuration class for the delete instrument profile page.
    /// </summary>
    internal class DeleteInstrumentProfilePageConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the delete instrument profile page.
        /// </summary>
        /// <returns>A JSON string that represents the page configuration.</returns>
        public string Get()
        {
            var page = @"{
                            name: 'deleteinstrumentprofilepage',
                            pageTitle: '@InsDel@',
                            text: '@InsDelB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'InstrumentName', type: 'text', label: '@InsProNam@', required: true, placeholder: '@InsEnt@'},
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

            return page;
        }
    }
}
