using arc.app.Common;

namespace arc.app.Config.Queries;

internal class EditIqcTestQcOrganismsQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "editiqctestqcorganismsquery",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
