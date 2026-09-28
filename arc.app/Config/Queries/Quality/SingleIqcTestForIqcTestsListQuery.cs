using arc.app.Common;

namespace arc.app.Config.Queries;

internal class SingleIqcTestForIqcTestsListQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "singleiqctestforiqctestslistquery",
                "TableName": "iqctests",
                "Type": "Single",
                "Fields": [
                    {
                        "Name": "Id",
                        "Type": "int"
                    },
                    {
                        "Name": "AccessionNumber",
                        "Type": "string"
                    },
                    {
                        "Name": "CreatedDate",
                        "Type": "datetime"
                    },
                    {
                        "Name": "CompletedDate",
                        "Type": "datetime"
                    },
                    {
                        "Name": "StateId",
                        "Type": "string"
                    }
                ],
                "Joins": [
                    {
                        "Table": "ListItem",
                        "Type": "Left",
                        "On": "StateId",
                        "From": "Id",
                        "Fields": [
                            {
                                "Name": "value",
                                "KnownAs": "State"
                            }
                        ]
                    }
                ],
                "Where": [
                    {
                        "Field": "Id",
                        "Comparison": "="
                    }
                ],
                "Translate": true
            }
            """;
    }
}
