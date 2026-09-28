using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the batch add patient tag page.
/// Allows selecting existing tags or creating new ones to add to multiple patients.
/// </summary>
internal class BatchAddPatientTagPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration.
    /// </summary>
    public string Get()
    {
        var page = @"{
                            name: 'batchaddpatienttagpage',
                            pageTitle: '@GenTagB@',
                            text: '@AleB@',
                            columns: [
                                {
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AddOnly', type: 'hidden', defaultValue: 'true' },
                                                { id: 'TagId', type: 'hierarchicalpicker', label: '@GenTagE@', placeholder: '@GenTagC@', optionsName: 'tag', dynamic: true, multiSelect: true },
                                                { id: 'TagName', type: 'singleline', label: '@GenTagD@', placeholder: '@GenTagD@' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
