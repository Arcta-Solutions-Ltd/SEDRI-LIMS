using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Result mapper definition for lite direct test list rows (title only).
/// </summary>
internal class TestListForSpecimenLiteResultMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                    'Name': 'testlistforspecimenliteresultmapper', 'Type': 'Special'
                 }";
    }
}
