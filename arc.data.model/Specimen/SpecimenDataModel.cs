namespace arc.data.model.Specimen;
/// <summary>
/// Represents the data model for a specimen.
/// Inherits from the IdAndDateBase class.
/// </summary>
public class SpecimenDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the patient ID.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the patient location ID.
    /// </summary>
    public int PatientLocationId { get; set; }

    /// <summary>
    /// Gets or sets the admission date.
    /// </summary>
    public DateTime AdmissionDate { get; set; }

    /// <summary>
    /// Gets or sets the diagnosis ID.
    /// </summary>
    public int DiagnosisId { get; set; }

    /// <summary>
    /// Gets or sets the clinical contact number.
    /// </summary>
    public string? ClinicalContactNo { get; set; }

    /// <summary>
    /// Gets or sets the accession number.
    /// </summary>
    public string? AccessionNumber { get; set; }

    /// <summary>
    /// Gets or sets the barcode.
    /// </summary>
    public string? Barcode { get; set; }

    /// <summary>
    /// Gets or sets the existing barcode.
    /// </summary>
    public string? ExistingBarcode { get; set; }

    /// <summary>
    /// Gets or sets the specimen type ID.
    /// </summary>
    public int SpecimenTypeId { get; set; }

    /// <summary>
    /// Gets or sets the specimen site ID.
    /// </summary>
    public int SpecimenSiteId { get; set; }

    /// <summary>
    /// Gets or sets the received condition ID.
    /// </summary>
    public int ReceivedConditionId { get; set; }

    /// <summary>
    /// Gets or sets the specimen appearance ID.
    /// </summary>
    public int SpecimenAppearanceId { get; set; }

    /// <summary>
    /// Gets or sets the specimen weight.
    /// </summary>
    public int SpecimenWeight { get; set; }

    /// <summary>
    /// Gets or sets the bottle-only weight.
    /// </summary>
    public decimal BottleOnlyWeight { get; set; }

    /// <summary>
    /// Gets or sets the blood and bottle weight.
    /// </summary>
    public decimal BloodAndBottleWeight { get; set; }

    /// <summary>
    /// Gets or sets the collection date.
    /// </summary>
    public DateTime CollectionDate { get; set; }

    /// <summary>
    /// Gets or sets the collection time.
    /// </summary>
    public string? CollectionTime { get; set; }

    /// <summary>
    /// Gets or sets the received date.
    /// </summary>
    public DateTime? ReceivedDate { get; set; }

    /// <summary>
    /// Gets or sets the received time.
    /// </summary>
    public string? ReceivedTime { get; set; }

    /// <summary>
    /// Gets or sets the state ID.
    /// </summary>
    public int StateId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specimen table. This links to the growth list in the listitem table.
    /// </summary>
    public int? GrowthId { get; set; }

    /// <summary>
    /// Gets or sets the rejection reason.
    /// </summary>
    public string? RejectionReason { get; set; }

    /// <summary>
    /// Gets or sets the age at specimen creation (years).
    /// </summary>
    public int? AgeYears { get; set; }

    /// <summary>
    /// Gets or sets the age at specimen creation (months).
    /// </summary>
    public int? AgeMonths { get; set; }

    /// <summary>
    /// Gets or sets the age at specimen creation (days).
    /// </summary>
    public int? AgeDays { get; set; }

    /// <summary>
    /// Gets or sets the age at specimen creation (hours).
    /// </summary>
    public int? AgeHours { get; set; }

    /// <summary>
    /// Gets or sets the approval comment level 1 ID.
    /// </summary>
    public int ApprovalCommentl1Id { get; set; }

    /// <summary>
    /// Gets or sets the approval comment level 2 ID.
    /// </summary>
    public int ApprovalCommentl2Id { get; set; }

    /// <summary>
    /// Gets or sets the first reason.
    /// </summary>
    public string? ReasonOne { get; set; }

    /// <summary>
    /// Gets or sets the second reason.
    /// </summary>
    public string? ReasonTwo { get; set; }

    /// <summary>
    /// Gets or sets the laboratory ID.
    /// </summary>
    public int LaboratoryId { get; set; }

    /// <summary>
    /// Gets or sets the organisation ID.
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Gets or sets additional data.
    /// </summary>
    public string? MoreData { get; set; }

    /// <summary>
    /// Gets or sets the alert type ID.
    /// </summary>
    public int AlertTypeId { get; set; }
}

