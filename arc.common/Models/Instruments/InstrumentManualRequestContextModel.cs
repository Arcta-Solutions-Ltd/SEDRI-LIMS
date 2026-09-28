namespace arc.common.Models.Instruments;

/// <summary>
/// Navigation context for the "request instrument test" form (embedded list origin). Used for client filtering and server validation.
/// When <see cref="ListKind"/> is <c>test</c>, InitialQuery adds <c>MatchesDirectTestContext</c> / <c>MatchesCultureTestContext</c>
/// on each instrument profile row (config ids resolved against the current test name).
/// </summary>
public class InstrumentManualRequestContextModel
{
    /// <summary>specimen | culture | test</summary>
    public string ListKind { get; set; }

    /// <summary>Parent record view name (e.g. specimenrecordview, cultures, testrecordview).</summary>
    public string RecordView { get; set; }

    public int SpecimenId { get; set; }
    public int CultureId { get; set; }
    public string LaboratoryId { get; set; }
    public string SpecimenTypeId { get; set; }
    public string CultureTypeId { get; set; }
    public string OrgGroupCodingId { get; set; }

    /// <summary>For test list: direct or culture.</summary>
    public string Source { get; set; }

    public string DirectTestName { get; set; }
    public string CultureTestName { get; set; }
}
