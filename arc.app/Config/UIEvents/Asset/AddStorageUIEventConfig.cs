using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the configuration for the "Add Storage UI Event".
/// </summary>
internal class AddStorageUIEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Add Storage UI Event".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Add Storage UI Event".
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addstorageuievent',
                        description: 'Add Storage',
                        type: 'form',
                        action: 'addstorageform'
                    }";

        return newEvent;
    }
}

