using arc.app.Common;

namespace arc.app.Config.Queries.Instruments;

/// <summary>
/// Query definition for loading the request instrument test form (context + profile list).
/// </summary>
internal class RequestInstrumentTestFormInitialQueryConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{
            'Query': 'RequestInstrumentTestFormInitialQuery',
            'TableName': 'instrumentresults',
            'Type': 'special'
        }";
    }
}
