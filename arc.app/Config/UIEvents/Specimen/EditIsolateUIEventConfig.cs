using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// UI event definition for launching the Edit Isolate form.
/// This event is referenced by buttons/menus with uievent 'editisolateuievent' and opens the 'editisolateform'.
/// </summary>
internal class EditIsolateUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the UI event that opens the Edit Isolate form.
    /// - name: 'editisolateuievent' (used by list/record view buttons)
    /// - type: 'form' (navigates to a form)
    /// - action: 'editisolateform' (form identifier to open)
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editisolateuievent',
                        description: 'Edit isolate',
                        type: 'form',
                        action: 'editisolateform'
                    }";

        return newEvent;
    }
}
