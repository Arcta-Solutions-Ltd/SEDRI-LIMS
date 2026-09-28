using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Provides the page configuration definition for the microscopy test page.
    /// This page contains fields for epithelium, bacteria, yeast identification,
    /// crystal and cast grids, and a print on report toggle field.
    /// </summary>
    internal class MicroscopyTestPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON string representation of the microscopy test page configuration.
        /// </summary>
        /// <returns>A JSON string containing the page configuration with combobox fields, field grids, and a print on report toggle.</returns>
        public string Get()
        {
            var page = @"{
                            name: 'microscopytestpage',
                            pageTitle: '@TesMic@',
                            text: '@TesEntN@.',
                            extrawide: false,
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
                                                { id: 'epitheliumId', type: 'combobox', label: '@TesEpiA@', optionsName: 'Epithelium'},
                                                { id: 'bacteriaId', type: 'combobox', label: '@GenBacA@', optionsName: 'seen'},
                                                { id: 'yeastId', type: 'combobox', label: '@GenYea@', optionsName: 'seen'},
                                                { id: 'crystalgrid', type: 'fieldgrid', label: '@GenCry@', gridfields: [
                                                        { id: 'crystal', type: 'dropdown', optionsName: 'crystal' },
                                                        { id: 'crystalseen', type: 'dropdown', optionsName: 'seen'  }
                                                    ]
                                                },
                                                { id: 'castgrid', type: 'fieldgrid', label: '@GenCas@', gridfields: [
                                                        { id: 'cast', type: 'dropdown', optionsName: 'cast' },
                                                        { id: 'castseen', type: 'dropdown', optionsName: 'seen'  }
                                                    ]
                                                },
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
