using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Represents the configuration for the "Delete Storage Page".
/// </summary>
internal class DeleteStoragePageConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Delete Storage Page".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Delete Storage Page".
    /// </returns>
    public string Get()
    {
        var page = @"{ 
                        name: 'deletestoragepage',
                        pageTitle: '@SupDelC@',
                        text: '@SupDelE@.',
                        columns: [
                            { 
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'StorageName', type: 'text', label: '@SupSto@' },
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
