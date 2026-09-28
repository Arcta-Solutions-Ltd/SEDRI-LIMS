using arc.app.Common;

namespace arc.app.Config.Pages.Export
{
    /// <summary>
    /// Page configuration for the crafted Manage Mapping editor (JSON or XML structural mapper).
    /// Uses the existing crafted page pipeline so that <see cref="arcportal"/> renders the
    /// MappingEditor React component for this page name.
    /// </summary>
    internal class ManageExportProfileMappingPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the page configuration as a JSON string.
        /// </summary>
        public string Get()
        {
            var page = @"{
                            name: 'manageexportprofilemappingpage',
                            pageTitle: '@ExpProMap@',
                            text: '@ExpProMapDesc@',
                            crafted: true,
                            nextButton: { show: true, buttonText: '@GenSav@' },
                            columns: [
                                {
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { Id: 'ExportProfileId', Type: 'hidden' },
                                                { Id: 'Format', Type: 'hidden' },
                                                { Id: 'Structure', Type: 'hidden' }
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
