using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Represents the configuration for the "Edit Supplier" page.
/// </summary>
internal class EditSupplierPageConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Edit Supplier" page.
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Edit Supplier" page.
    /// </returns>
    public string Get()
    {
        var page = @"{ 
                        name: 'editsupplierpage',
                        pageTitle: '@SupEdi@',
                        text: '@SupEdiB@.',
                        columns: [
                            { 
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'Name', type: 'singleline', label: '@SupSup@', required: true, placeholder: '@GenNam@', Max: 99 },
                                            { id: 'Code', type: 'singleline', label: '@GenCodA@', required: true, placeholder: '@GenCodA@', Max: 30},
                                            { id: 'AddressLine1', type: 'singleline', label: '@PatAddD@', Max: 50 },
                                            { id: 'AddressLine2', type: 'singleline', label: '@PatAddE@', Max: 50 },
                                            { id: 'LocationId', type: 'hierarchy', placeholder: '@LocSel@', label: '@GenLoc@', optionsName: 'LocationList' },
                                            { id: 'ZipCode', type: 'singleline', label: '@PatZip@', Max: 50 },
                                            { id: 'SupplierStatusId', type: 'dropdown', label: '@GenStaA@', placeholder: '@GenStaA@', optionsName: 'SupplierStatus', required: true }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}

