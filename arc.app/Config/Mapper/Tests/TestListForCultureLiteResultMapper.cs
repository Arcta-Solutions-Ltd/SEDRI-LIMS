using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Result mapper definition for lite isolate test list rows (title only).
/// </summary>
internal class TestListForCultureLiteResultMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                    'Name': 'testlistforcultureliteresultmapper', 'Type': 'Special'
                 }";
    }
}
