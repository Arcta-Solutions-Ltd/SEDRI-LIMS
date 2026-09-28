using arc.app.Common;

namespace arc.app.Config.Queries;

internal class EditIqcTestProfileQcOrganismQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "editiqctestprofileqcorganismquery",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
