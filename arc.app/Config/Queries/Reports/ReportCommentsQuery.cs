using arc.app.Common;

namespace arc.app.Config.Queries;

internal class ReportCommentsQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "reportcomments",
                "TableName": "SpecimenComment",
                "Type": "Special",
                "Translate": true
            }
            """;
    }
}
