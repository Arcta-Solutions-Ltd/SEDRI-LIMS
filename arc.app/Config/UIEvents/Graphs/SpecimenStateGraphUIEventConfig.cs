using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the UI event configuration for the Specimen State summary graph (workflow states).
/// </summary>
internal class SpecimenStateGraphUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration string for the specimen state graph UI event.
    /// </summary>
    /// <returns>Event name, description, type, and graph action identifier.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'specimenstategraphuievent',
                        description: 'Specimen State Graph',
                        type: 'graph',
                        action: 'specimenstategraph'
                    }";

        return newEvent;
    }
}
