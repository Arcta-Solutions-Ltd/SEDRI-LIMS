using arc.app.Common;

namespace arc.app.Config.Queries;

internal class IqcResultsGridQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "iqcresultsgridquery",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
