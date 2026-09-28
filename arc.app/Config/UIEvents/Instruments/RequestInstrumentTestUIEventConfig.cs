using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>Opens the request instrument test form from embedded instrument result lists.</summary>
internal class RequestInstrumentTestUIEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{
            name: 'requestinstrumenttestuievent',
            description: '@InsReqEv@',
            type: 'form',
            action: 'requestinstrumenttestform',
            recordView: 'specimenrecordview'
        }";
    }
}
