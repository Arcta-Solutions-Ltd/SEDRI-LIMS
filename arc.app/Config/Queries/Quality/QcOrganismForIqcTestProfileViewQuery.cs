using arc.app.Common;

namespace arc.app.Config.Queries;

internal class QcOrganismForIqcTestProfileViewQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "qcorganismforiqctestprofileview",
                "Type": "special",
                "Translate": true,
                "ResultMapping": "qcorganismviewmapper"
            }
            """;
    }
}
