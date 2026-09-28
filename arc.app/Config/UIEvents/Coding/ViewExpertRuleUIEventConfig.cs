using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding;

/// <summary>
/// Defines the UI event configuration used for viewing expert rule details.
/// </summary>
internal class ViewExpertRuleUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the JSON configuration for the 'view expert rule' UI event.
    /// The configuration specifies the event name, description, type, and action.
    /// </summary>
    /// <returns>A JSON string defining the UI event for viewing expert rule details.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'viewexpertruleuievent',
                        description: 'View expert rule details',
                        type: 'view-record',
                        action: 'expertrules'
                    }";

        return newEvent;
    }
}
