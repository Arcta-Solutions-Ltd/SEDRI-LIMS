using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Configuration for ESBL Test Page.
    /// </summary>
    internal class EsblTestPageConfig : IDefinition
    {
        /// <summary>
        /// Gets the configuration for the ESBL Test Page.
        /// </summary>
        /// <returns>A string containing the ESBL Test Page configuration in JSON format.</returns>
        public string Get()
        {
            var page = @"{ 
                        name: 'esbltestpage',
                        pageTitle: '@TesEsb@',
                        text: '@TesEntR@.',
                        configureActions: 'add,edit',
                        columns: [
                            { 
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'esblresultId', type: 'combobox', label: '@TesEsbA@', optionsName: 'esbl', required: true},
                                            { id: 'printonreport', type: 'toggle', label: '@GenDis@', defaultValue: 'Yes', Configurable: 'No'}
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

            return page;
        }
    }
}
