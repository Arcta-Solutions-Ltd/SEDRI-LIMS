using arc.app.Common;

namespace arc.app.Config.Queries;

internal class IqcTestProfileQcAntibioticsDiskQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "iqctestprofileqcantibioticsdisk",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
