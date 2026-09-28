using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the configuration for the Add Supplier UI event.
/// </summary>
internal class AddSupplierUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Add Supplier UI event.
    /// </summary>
    /// <returns>A JSON string representing the Add Supplier UI event configuration.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addsupplieruievent',
                        description: 'Add Supplier',
                        type: 'form',
                        action: 'addsupplierform'
                    }";

        return newEvent;
    }
}

