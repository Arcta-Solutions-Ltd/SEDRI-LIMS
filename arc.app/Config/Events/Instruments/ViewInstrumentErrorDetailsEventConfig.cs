using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration class for the view instrument error details event.
/// </summary>
internal class ViewInstrumentErrorDetailsEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON string for the view instrument error details event.
    /// </summary>
    /// <returns>A JSON string that represents the event configuration.</returns>
    public string Get()
    {
        return @"{ 
                        EventName: 'viewinstrumenterrordetails', 
                        Description: '@InsInsC@',
                        Topic : 'Instruments',
                        EventType : 'special'
                    }";
    }
}
