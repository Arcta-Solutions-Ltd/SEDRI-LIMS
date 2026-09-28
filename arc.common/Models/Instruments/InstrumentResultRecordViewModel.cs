using Newtonsoft.Json;

namespace arc.common.Models.Instruments;

/// <summary>
/// Read-only row for the instrument result record view (by instrument result id).
/// </summary>
public class InstrumentResultRecordViewModel
{
    /// <summary>Internal key for queries; omitted from API payload so the record view does not show instrument result id.</summary>
    [JsonIgnore]
    public int Id { get; set; }
    public string InstrumentProfile { get; set; } = "";
    /// <summary>Patient display name (same pattern as instrument results list).</summary>
    public string PatientName { get; set; } = "";
    /// <summary>Listitem display value for specimen type.</summary>
    public string SpecimenType { get; set; } = "";
    /// <summary>Listitem display value for culture/isolate type (empty when no culture).</summary>
    public string CultureType { get; set; } = "";
    public string Barcode { get; set; } = "";
    public string RequestMade { get; set; } = "";
    public string ResultReceived { get; set; } = "";
    public string Status { get; set; } = "";
    public string RawResult { get; set; } = "";
    public string LastModifiedDate { get; set; } = "";
    public string AccessionNumber { get; set; } = "";
}
