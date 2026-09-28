using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the configuration for a UI event to view the details 
/// of an instrument error. This class implements the <see cref="IDefinition"/> interface.
/// </summary>
/// <remarks>
/// The <c>Get</c> method returns a JSON-formatted string that specifies 
/// the event's name, description, type, and associated action.
/// </remarks>
/// <returns>
/// A JSON string representing the configuration of the UI event.
/// </returns>
internal class ViewInstrumentErrorDetailsUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'viewinstrumenterrordetailsuievent',
                        description: '@InsViewB@',
                        type: 'form',
                        action: 'viewinstrumenterrordetailsform'
                    }";

        return newEvent;
    }
}

