using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>Event configuration for manually requesting an instrument test from embedded specimen/culture/test lists.</summary>
internal class RequestInstrumentTestEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{
            EventName: 'requestinstrumenttest',
            Description: '@InsReqEv@',
            EventType: 'special',
            Topic: 'Instruments',
            TableName: 'instrumentresults',
            ValidationRules: [
                { field: 'InstrumentProfileId', rule: 'required', message: '@GenReqB@' }
            ]
        }";
    }
}
