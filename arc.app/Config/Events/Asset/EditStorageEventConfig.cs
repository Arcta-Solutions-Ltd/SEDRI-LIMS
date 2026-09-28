using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the configuration for the "Edit Storage Event".
/// </summary>
internal class EditStorageEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Edit Storage Event".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Edit Storage Event".
    /// </returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'editstorageevent', 
                    Description: '@SupEdiD@',
                    EventType : 'special', 
                    Topic : 'Asset', 
                    TableName: 'Storage',
                    ValidationRules: [
                        { field: 'StorageName', rule: 'required', message: '@SupYou@'},
                        { field: 'StorageTypeId', rule: 'required', message: '@SupYouA@'}
                    ]
                }";
    }
}

