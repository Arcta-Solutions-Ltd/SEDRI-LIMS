using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Represents the configuration for the "Delete Supplier" page.
/// </summary>
internal class DeleteSupplierPageConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Delete Supplier" page.
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Delete Supplier" page.
    /// </returns>
    public string Get()
    {
        var page = @"{ 
                        name: 'deletesupplierpage',
                        pageTitle: '@SupDel@',
                        text: '@SupDelB@.',
                        columns: [
                            { 
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'Name', type: 'text', label: '@SupSup@' },
                                            { id: 'Code', type: 'text', label: '@GenCodA@'},
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
