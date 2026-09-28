using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the configuration for the Edit Supplier UI event.
/// </summary>
internal class EditSupplierUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Edit Supplier UI event.
    /// </summary>
    /// <returns>A JSON string representing the Edit Supplier UI event configuration.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editsupplieruievent',
                        description: 'Edit Supplier',
                        type: 'form',
                        action: 'editsupplierform'
                    }";

        return newEvent;
    }
}

