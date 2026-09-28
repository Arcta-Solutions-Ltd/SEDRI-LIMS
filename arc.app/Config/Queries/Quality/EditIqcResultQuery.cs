using arc.app.Common;
namespace arc.app.Config.Queries;

internal class EditIqcResultQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "editiqcresult",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
