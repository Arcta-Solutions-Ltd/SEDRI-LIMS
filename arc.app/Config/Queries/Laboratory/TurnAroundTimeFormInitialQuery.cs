using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Configuration for the "Turn Around Time Form Initial" query.
/// Executed via SpecialQueryFactory (TurnAroundTimeFormInitialQueryHandler).
/// </summary>
internal class TurnAroundTimeFormInitialQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration.
    /// </summary>
    public string Get()
    {
        return @"{
                        'Query': 'TurnAroundTimeFormInitialQuery',
                        'Type': 'special'
                    }";
    }
}
