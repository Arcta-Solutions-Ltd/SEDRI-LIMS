using arc.app.Common;

namespace arc.app.Config.Queries;

internal class RunIqcTestQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "runiqctestquery",
                "TableName": "iqctests",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
