using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the configuration for the edit instrument profile UI event.
/// </summary>
internal class EditInstrumentProfileUIEventConfig : IDefinition
{
    /// <summary>
    /// Gets the configuration for the edit instrument profile UI event as a JSON string.
    /// </summary>
    /// <returns>The configuration JSON string.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editinstrumentprofileuievent',
                        description: '@InsEdiC@',
                        type: 'form',
                        action: 'editinstrumentprofileform'
                    }";

        return newEvent;
    }
}

