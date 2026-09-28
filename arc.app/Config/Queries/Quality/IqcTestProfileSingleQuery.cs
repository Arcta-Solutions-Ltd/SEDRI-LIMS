using arc.app.Common;

namespace arc.app.Config.Queries;

internal class IqcTestProfileSingleQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "iqctestprofilesinglequery",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
