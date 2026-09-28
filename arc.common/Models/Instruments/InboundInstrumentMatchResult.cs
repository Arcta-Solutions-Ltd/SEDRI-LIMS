using arc.data.model.Instruments;

namespace arc.common.Models.Instruments;

/// <summary>
/// Result of resolving an <c>instrumentresults</c> row for inbound instrument payloads.
/// </summary>
public sealed class InboundInstrumentMatchResult
{
    /// <summary>Matched row, or null when not found or ambiguous.</summary>
    public InstrumentResultsDataModel Row { get; init; }

    /// <summary>More than one row matched composite criteria.</summary>
    public bool IsAmbiguous { get; init; }
}
