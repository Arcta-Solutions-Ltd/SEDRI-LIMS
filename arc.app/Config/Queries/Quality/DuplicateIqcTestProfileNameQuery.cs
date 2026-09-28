using arc.app.Common;

namespace arc.app.Config.Queries;

internal class DuplicateIqcTestProfileNameQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "duplicateiqctestprofilenamequery",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
