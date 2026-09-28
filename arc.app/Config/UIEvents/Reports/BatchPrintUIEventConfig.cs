using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides the configuration for the BatchPrint UI event.
/// </summary>
internal class BatchPrintUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON representation of the BatchPrint UI event.
    /// </summary>
    /// <returns>A JSON string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
            name: 'batchprintuievent',
            description: 'Batch Print',
            type: 'form',
            action: 'batchprintform'
        }";

        return newEvent;
    }
}
