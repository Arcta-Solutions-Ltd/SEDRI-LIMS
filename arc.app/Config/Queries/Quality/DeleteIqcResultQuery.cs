using arc.app.Common;
namespace arc.app.Config.Queries;

internal class DeleteIqcResultQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "deleteiqcresultquery",
                "TableName": "iqcresults",
                "Type": "Single",
                "Fields": [
                    {
                        "Name": "id",
                        "Type": "string"
                    },
                    {
                        "Name": "value",
                        "Type": "string"
                    }
                ],
                "Where": [
                    {
                        "Field": "id",
                        "Comparison": "="
                    }
                ],
                "Joins": [
                    {
                        "Table": "qcantibiotics",
                        "Type": "Left",
                        "On": "qcantibioticid",
                        "From": "id"
                    },
                    {
                        "Table": "antibiotic",
                        "Type": "Left",
                        "On": "a.antibioticid",
                        "From": "id",
                        "Fields": [
                            {
                                "Name": "antibioticname"
                            }
                        ]
                    }
                ]
            }
            """;
    }
}
