using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// UI event for launching the Delete Culture form.
/// Triggered by 'deletecultureuievent' and opens the 'deletecultureform'.
/// </summary>
internal class DeleteCultureUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the delete culture UI event.
    /// - name: 'deletecultureuievent'
    /// - type: 'form'
    /// - action: 'deletecultureform'
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deletecultureuievent',
                        description: 'Delete Culture',
                        type: 'form',
                        action: 'deletecultureform'
                    }";

        return newEvent;
    }
}
