using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the configuration for the "Edit Storage UI Event".
/// </summary>
internal class EditStorageUIEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Edit Storage UI Event".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Edit Storage UI Event".
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editstorageuievent',
                        description: 'Edit Storage',
                        type: 'form',
                        action: 'editstorageform'
                    }";

        return newEvent;
    }
}

