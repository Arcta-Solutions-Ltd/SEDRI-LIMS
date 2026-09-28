using arc.app.Common;

namespace arc.app.Config.Queries;

internal class IqcTestsListQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "iqctestslist",
                "TableName": "iqctests",
                "Type": "Special",
                "Translate": true
            }
            """;
    }
}
