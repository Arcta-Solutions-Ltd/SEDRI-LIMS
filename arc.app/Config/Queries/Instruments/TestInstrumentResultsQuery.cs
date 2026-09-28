using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query definition JSON for the test record view instrument results embedded list.
/// </summary>
internal class TestInstrumentResultsQuery : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{ 
                        'Query': 'TestInstrumentResultsQuery',
                        'TableName': 'InstrumentResults',
                        'Translate': false,
                        'Type': 'special'
                    }";
    }
}
