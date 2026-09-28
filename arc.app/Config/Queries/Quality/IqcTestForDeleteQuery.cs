using arc.app.Common;

namespace arc.app.Config.Queries;

internal class IqcTestForDeleteQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "iqctestfordeletequery",
                "TableName": "iqctests",
                "Type": "Single",
                "Fields": [
                    {
                        "Name": "id",
                        "Type": "int"
                    },
                    {
                        "Name": "accessionnumber",
                        "Type": "string"
                    },
                    {
                        "Name": "createddate",
                        "Type": "date"
                    },
                    {
                        "Name": "completeddate",
                        "Type": "date"
                    },
                    {
                        "Name": "stateid",
                        "Type": "string"
                    }
                ],
                "Joins": [
                    {
                        "Table": "ListItem",
                        "Type": "Left",
                        "On": "stateid",
                        "From": "Id",
                        "Fields": [
                            {
                                "Name": "value",
                                "KnownAs": "state"
                            }
                        ]
                    }
                ],
                "Where": [
                    {
                        "Field": "id",
                        "Comparison": "="
                    }
                ],
                "Orderby": "id",
                "Translate": true
            }
            """;
    }
}
