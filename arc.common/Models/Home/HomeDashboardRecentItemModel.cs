namespace arc.common.Models.Home;

/// <summary>
/// One row for the home dashboard Recently Used visualization (from queue-derived activity).
/// Display names for specimen type etc. are resolved client-side from list ids where applicable.
/// </summary>
public class HomeDashboardRecentItemModel
{
    /// <summary>Discriminant: specimen, patient, test, or culture (isolate).</summary>
    public string Kind { get; set; } = "";

    public int SpecimenId { get; set; }

    public int PatientId { get; set; }

    /// <summary>Tests.Id when <see cref="Kind"/> is test.</summary>
    public int TestId { get; set; }

    /// <summary>Culture.Id when <see cref="Kind"/> is culture.</summary>
    public int CultureId { get; set; }

    /// <summary>ListItem id for culture type when <see cref="Kind"/> is culture.</summary>
    public int? CultureTypeId { get; set; }

    /// <summary>Isolate/culture number when <see cref="Kind"/> is culture.</summary>
    public int? CultureNumber { get; set; }

    public string AccessionNumber { get; set; } = "";

    /// <summary>ListItem id for specimen type when applicable.</summary>
    public int? SpecimenTypeId { get; set; }

    public string PatientRef { get; set; } = "";

    public string PatientFirstName { get; set; } = "";

    public string PatientSurname { get; set; } = "";

    /// <summary>Tests.TestName for direct tests (canonical config name, not a translated label).</summary>
    public string TestName { get; set; } = "";
}
