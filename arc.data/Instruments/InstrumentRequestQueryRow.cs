using System;

namespace arc.data.Instruments;

/// <summary>
/// Dapper row for <see cref="InstrumentRequestQuery"/> before mapping to <see cref="arc.common.Models.Instruments.InstrumentRequestModel"/>.
/// </summary>
internal sealed class InstrumentRequestQueryRow
{
    public int Id { get; set; }
    public int SpecimenId { get; set; }
    public int CultureId { get; set; }
    public string AccessionNumber { get; set; } = "";
    public string PatientRef { get; set; } = "";
    public string Barcode { get; set; } = "";
    public string CultureNumber { get; set; } = "";
    public string OrganismCode { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int? InstrumentMachineId { get; set; }
    public string InstrumentName { get; set; } = "";
    public string CollectedDateTime { get; set; } = "";
    public string SpecimenTypeName { get; set; } = "";
    public DateTime? BlaCompleted { get; set; }
    public string BlaStatus { get; set; } = "";
    public string BlaListItemValue { get; set; } = "";
}
