using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the configuration for the "Add Storage Event".
/// </summary>
internal class AddStorageEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Add Storage Event".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Add Storage Event".
    /// </returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'addstorageevent', 
                    Description: '@SupAddC@',
                    EventType : 'specialadddata', 
                    Topic : 'Asset', 
                    TableName: 'Storage',
                    ValidationRules: [
                        { field: 'StorageName', rule: 'required', message: '@SupYou@'},
                        { field: 'StorageTypeId', rule: 'required', message: '@SupYouA@'}
                    ]
                }";
    }
}

