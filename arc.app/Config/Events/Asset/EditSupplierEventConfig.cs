using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the configuration for the Edit Supplier event.
/// </summary>
internal class EditSupplierEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Edit Supplier event.
    /// </summary>
    /// <returns>A JSON string representing the Edit Supplier event configuration.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'editsupplierevent', 
                    Description: '@SupEdiA@',
                    EventType : 'editdata', 
                    Topic : 'Asset', 
                    TableName: 'Supplier',
                    ValidationRules: [
                        { field: 'Name', rule: 'required', message: '@SupYou@'},
                        { field: 'SupplierStatusId', rule: 'required', message: '@SupYouA@'}
                    ]
                }";
    }
}

