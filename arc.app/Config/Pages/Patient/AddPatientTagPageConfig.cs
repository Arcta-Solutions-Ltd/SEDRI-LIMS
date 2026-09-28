using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Add Patient Tag" page.
/// </summary>
internal class AddPatientTagPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration.
    /// </summary>
    public string Get()
    {
        var page = @"{
                            name: 'addpatienttagpage',
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
