using arc.app.Common;

namespace arc.app.Config.Queries;

internal class AddIqcTestQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "addiqctestquery",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
