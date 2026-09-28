using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Represents the configuration for the "Add Storage Page".
/// </summary>
internal class AddStoragePageConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Add Storage Page".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Add Storage Page".
    /// </returns>
    public string Get()
    {
        var page = @"{ 
                        name: 'addstoragepage',
                        pageTitle: '@SupAddB@',
                        text: '@SupEntA@.',
                        columns: [
                            { 
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'StorageName', type: 'singleline', label: '@SupSto@', required: true, placeholder: '@GenNam@', Max: 99 },
                                            { id: 'Description', type: 'multiline', label: '@ExpProD@', placeholder: '@ExpProD@', Max: 499 },
                                            { id: 'Code', type: 'singleline', label: '@GenCodA@', placeholder: '@GenCodA@', Max: 30},
                                            { id: 'Temperature', type: 'number', label: '@PatZip@', Max: 50 },
                                            { id: 'StorageTypeId', type: 'dropdown', label: '@SupStoA@', placeholder: '@SupStoA@', optionsName: 'StorageType', required: true },
                                            { id: 'ParentStorageId', type: 'dropdown', label: '@SupPar@', placeholder: '@SupPar@', optionsName: 'StorageList', dynamic: true},
                                            { id: 'enabled', type: 'toggle', label: '@GenEna@', required: true, value: 'Yes', defaultValue: 'Yes' }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}

