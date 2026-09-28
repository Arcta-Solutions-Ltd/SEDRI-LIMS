using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the configuration for the "Delete Storage Event".
/// </summary>
internal class DeleteStorageEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Delete Storage Event".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Delete Storage Event".
    /// </returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'deletestorageevent', 
                    Description: '@SupDelD@',
                    EventType : 'deletedata', 
                    Topic : 'Asset', 
                    TableName: 'Storage',
                    DataRules: [
                        { type: 'NoRecord', query: 'storageparentcountquery', message: '@SupThi@' }
                    ]
                }";
    }
}

