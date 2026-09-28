using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// UI event definition for deleting an isolate.
/// Triggered by 'deleteisolateuievent' and opens the 'deleteisolateform'.
/// </summary>
internal class DeleteIsolateUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the delete isolate UI event.
    /// - name: 'deleteisolateuievent'
    /// - type: 'form'
    /// - action: 'deleteisolateform'
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deleteisolateuievent',
                        description: 'Delete Isolate',
                        type: 'form',
                        action: 'deleteisolateform'
                    }";

        return newEvent;
    }
}
