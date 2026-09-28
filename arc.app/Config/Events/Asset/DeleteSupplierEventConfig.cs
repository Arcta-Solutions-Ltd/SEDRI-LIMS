using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Represents the configuration for the Delete Supplier event.
/// </summary>
internal class DeleteSupplierEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Delete Supplier event.
    /// </summary>
    /// <returns>A JSON string representing the Delete Supplier event configuration.</returns>
    public string Get()
    {
        return @"{ 
                    EventName: 'deletesupplierevent', 
                    Description: '@SupDelA@',
                    EventType : 'deletedata', 
                    Topic : 'Asset', 
                    TableName: 'Supplier'
                }";
    }
}

