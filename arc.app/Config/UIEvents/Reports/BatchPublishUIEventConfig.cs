using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides the configuration for the BatchPublish UI event.
/// </summary>
internal class BatchPublishUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON representation of the BatchPublish UI event.
    /// </summary>
    /// <returns>A JSON string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
            name: 'batchpublishuievent',
            description: 'Batch Publish',
            type: 'form',
            action: 'batchpublishform'
        }";

        return newEvent;
    }
}
