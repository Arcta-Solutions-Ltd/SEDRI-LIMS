using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides the configuration for the CulturePrintSelector UI event.
/// </summary>
internal class CulturePrintSelectorUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON representation of the CulturePrintSelector UI event.
    /// </summary>
    /// <returns>A JSON string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
            name: 'cultureprintselectoruievent',
            description: 'Print Selector',
            type: 'form',
            action: 'cultureprintselectorform'
        }";

        return newEvent;
    }
}
