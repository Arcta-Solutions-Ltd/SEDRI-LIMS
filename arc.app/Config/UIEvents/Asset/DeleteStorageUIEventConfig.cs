using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the configuration for the "Delete Storage UI Event".
/// </summary>
internal class DeleteStorageUIEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Delete Storage UI Event".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Delete Storage UI Event".
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deletestorageuievent',
                        description: 'Delete Storage',
                        type: 'form',
                        action: 'deletestorageform'
                    }";

        return newEvent;
    }
}

