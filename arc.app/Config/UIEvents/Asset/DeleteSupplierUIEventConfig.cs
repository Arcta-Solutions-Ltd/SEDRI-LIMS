using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the configuration for the Delete Supplier UI event.
/// </summary>
internal class DeleteSupplierUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Delete Supplier UI event.
    /// </summary>
    /// <returns>A JSON string representing the Delete Supplier UI event configuration.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deletesupplieruievent',
                        description: 'Delete Supplier',
                        type: 'form',
                        action: 'deletesupplierform'
                    }";

        return newEvent;
    }
}

