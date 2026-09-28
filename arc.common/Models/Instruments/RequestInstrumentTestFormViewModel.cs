namespace arc.common.Models.Instruments;

/// <summary>
/// Payload for saving the request instrument test form.
/// </summary>
public class RequestInstrumentTestFormViewModel
{
    /// <summary>Selected profile id from <see cref="SingleInstrumentConfig.Id"/> (or fallback key).</summary>
    public string InstrumentProfileId { get; set; }

    /// <summary>Parent id for the embedded list (specimen, culture, or test row id).</summary>
    public int MetaParentId { get; set; }

    /// <summary>Record view name (e.g. specimenrecordview, cultures, testrecordview).</summary>
    public string MetaRecordView { get; set; }

    /// <summary>For test record view: direct or culture.</summary>
    public string MetaSource { get; set; }
}
