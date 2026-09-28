using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the UI event configuration for acknowledging specimen receipt.
/// Implements <see cref="IDefinition"/> to supply the event metadata.
/// </summary>
internal class ACKReceiptUIEventConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON string that describes the ackreceiptuievent configuration.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing:
    /// - name: 'ackreceiptuievent'
    /// - description: 'Acknowledge specimen receipt'
    /// - type: 'form'
    /// - action: 'ackreceiptform'
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'ackreceiptuievent',
                        description: 'Acknowledge specimen receipt',
                        type: 'form',
                        action: 'ackreceiptform'
                    }";

        return newEvent;
    }
}
