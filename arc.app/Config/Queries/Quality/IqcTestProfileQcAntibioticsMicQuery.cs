using arc.app.Common;

namespace arc.app.Config.Queries;

internal class IqcTestProfileQcAntibioticsMicQuery : IDefinition
{
    public string Get()
    {
        return """
            {
                "Query": "iqctestprofileqcantibioticsmic",
                "Type": "special",
                "Translate": true
            }
            """;
    }
}
