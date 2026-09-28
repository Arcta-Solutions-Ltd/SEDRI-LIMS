using arc.app.Common;

namespace arc.app.Config.Pages.Config
{
    /// <summary>
    /// Page definition for the Add Existing Field form. Renders the reuse candidate picker fed by
    /// <c>existingFieldOptions</c> from the initial query.
    /// </summary>
    internal class AddExistingFieldPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON configuration string for the add existing field page.
        /// </summary>
        /// <returns>A JSON string defining the add existing field page.</returns>
        public string Get()
        {
            var page = @"{
                            name: 'addexistingfieldpage',
                            pageTitle: '@ConAddEF@',
                            text: '@ConAddEFA@',
                            wider: true,
                            tableName: 'None',
                            configureActions: '',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'TargetPageTitle', type: 'text', label: '@GenPagA@', Configurable: 'No' },
                                                { id: 'ExistingFieldIds', type: 'existingfieldselector', label: '@ConExiF@', required: true, Configurable: 'No', optionsName: 'existingFieldOptions' }
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
