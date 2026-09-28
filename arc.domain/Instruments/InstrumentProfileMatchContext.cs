namespace arc.domain.Instruments;

/// <summary>
/// Laboratory/specimen/culture facts used to decide whether an <see cref="InstrumentConfig"/> profile row
/// should create a pending <c>instrumentresults</c> row. Optional fields are only set when relevant to the trigger.
/// </summary>
public class InstrumentProfileMatchContext
{
    public string LaboratoryId { get; set; }
    public string SpecimenTypeId { get; set; }
    /// <summary>Culture type list item id when a culture is in scope.</summary>
    public string CultureTypeId { get; set; }
    /// <summary>Direct test configname (Tests.testname) when the trigger is a direct test.</summary>
    public string DirectTestName { get; set; }
    /// <summary>Culture/isolate test name when the trigger is a culture test.</summary>
    public string CultureTestName { get; set; }
    /// <summary>Culture.OrgGroupCodingId when an organism/group is in scope (string for list id).</summary>
    public string OrgGroupCodingId { get; set; }
    public string Barcode { get; set; }
    public int SpecimenId { get; set; }
    public int CultureId { get; set; }
}
