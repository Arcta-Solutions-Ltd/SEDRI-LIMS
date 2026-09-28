using arc.app.Common;

namespace arc.app.Config.Queries;

internal class MarkIqcTestCompleteQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "markiqctestcompletequery",
                "TableName": "iqctests",
                "Type": "Single",
                "Fields": [
                    {
                        "Name": "Id",
                        "Type": "int"
                    },
                    {
                        "Name": "StateId",
                        "Type": "int"
                    }
                ],
                "Where": [
                    {
                        "Field": "Id",
                        "Comparison": "="
                    }
                ]
            }
            """;
    }
}
