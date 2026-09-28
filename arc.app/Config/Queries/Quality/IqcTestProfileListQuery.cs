using arc.app.Common;

namespace arc.app.Config.Queries;

internal class IqcTestProfileListQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "iqctestprofilelistquery",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
