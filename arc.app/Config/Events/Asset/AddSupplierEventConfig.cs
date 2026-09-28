using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the configuration for the Add Supplier event.
/// </summary>
internal class AddSupplierEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Add Supplier event.
    /// </summary>
    /// <returns>A JSON string representing the Add Supplier event configuration.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'addsupplierevent', 
                    Description: '@SupAddA@',
                    EventType : 'adddata', 
                    Topic : 'Asset', 
                    TableName: 'Supplier',
                    ValidationRules: [
                        { field: 'Name', rule: 'required', message: '@SupYou@'},
                        { field: 'SupplierStatusId', rule: 'required', message: '@SupYouA@'}
                    ]
                }";
    }
}

