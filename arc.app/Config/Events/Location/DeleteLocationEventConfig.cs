using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the configuration for the "Delete Location" event.
/// </summary>
internal class DeleteLocationEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Delete Location" event.
    /// </summary>
    /// <returns>
    /// A JSON string defining the event configuration, including validation rules and data rules for deletion.
    /// </returns>
    public string Get()
    {
        return @"{ 
                        EventName: 'deleteLocation', 
                        Description: '@LocDel@',
                        EventType : 'deletedata', 
                        Topic : 'Location', 
                        TableName: 'Location',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@LocA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'locationparentcount', message: '@LocThi@' },
                            { type: 'NoRecord', query: 'locationpatientcount', message: '@LocThiA@' },
                            { type: 'NoRecord', query: 'locationsuppliercountquery', message: '@LocThiB@' }
                        ]
                    }";
    }
}

