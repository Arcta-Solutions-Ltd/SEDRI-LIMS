using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace arc.data.Instruments;

/// <summary>
/// Builds JSON fragments for Vitek 2 outbound LIS XML mapping (MachineIntegrationService).
/// </summary>
internal static class VitekOutboundInstrumentRequestBuilder
{
    private static readonly JsonSerializerSettings CamelCaseSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    /// <summary>
    /// Returns a JSON object string for the Bla offline test element, or "{}" when omitted (empty XML &lt;test&gt;&lt;/test&gt;).
    /// </summary>
    public static string BuildBlaTestJson(DateTime? blaCompleted, string blaStatus, string blaListItemValue)
    {
        if (blaCompleted == null || string.IsNullOrWhiteSpace(blaStatus)
            || !string.Equals(blaStatus.Trim(), "Complete", StringComparison.OrdinalIgnoreCase))
            return "{}";

        var code = ClassifyBlaResultCode(blaListItemValue);
        if (code == null)
            return "{}";

        var payload = new
        {
            universalIdentifier = new { testIdentifier = "Bla", testName = "BetaLacamase" },
            result = new
            {
                value = new
                {
                    offlineTest = new { resultCode = code }
                }
            }
        };

        return JsonConvert.SerializeObject(payload, CamelCaseSettings);
    }

    /// <summary>
    /// Maps list item display text to Pos/Neg, or null if ambiguous.
    /// </summary>
    internal static string ClassifyBlaResultCode(string listItemValue)
    {
        if (string.IsNullOrWhiteSpace(listItemValue))
            return null;

        var v = listItemValue.Trim().ToLowerInvariant();

        if (v.Contains("negative", StringComparison.Ordinal) || v == "neg" || v.StartsWith("neg ", StringComparison.Ordinal))
            return "Neg";

        if (v.Contains("positive", StringComparison.Ordinal) || v == "pos" || v.StartsWith("pos ", StringComparison.Ordinal))
            return "Pos";

        return null;
    }
}
