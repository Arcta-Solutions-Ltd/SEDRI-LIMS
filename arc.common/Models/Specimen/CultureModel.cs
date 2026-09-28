using System.Collections.Generic;

namespace arc.common.Models.Specimen;

/// <summary>
/// Represents a microbiology culture record, including organism identification,
/// specimen details, and associated metadata for reporting and analysis.
/// </summary>
public class CultureModel
{
    /// <summary>
    /// Unique identifier for the culture record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Type of culture performed (e.g., blood, urine, wound).
    /// </summary>
    public string CultureType { get; set; }

    /// <summary>
    /// Identifier for the detected organism.
    /// </summary>
    public string OrganismId { get; set; }

    /// <summary>
    /// Name of the detected organism.
    /// </summary>
    public string OrganismName { get; set; }

    /// <summary>
    /// Coding used to group organisms for classification or reporting.
    /// </summary>
    public string OrgGroupCoding { get; set; }

    /// <summary>
    /// Quantity or concentration of the organism detected.
    /// </summary>
    public string Quantity { get; set; }

    /// <summary>
    /// Date when the culture tested positive.
    /// </summary>
    public string PositiveDate { get; set; }

    /// <summary>
    /// Time when the culture tested positive.
    /// </summary>
    public string PositiveTime { get; set; }

    /// <summary>
    /// Indicates whether this culture result should be displayed in the final report.
    /// </summary>
    public string DisplayInReport { get; set; }

    /// <summary>
    /// Identifier for the aliquot (sample portion) used in the culture.
    /// </summary>
    public string AliquotID { get; set; }

    /// <summary>
    /// Additional notes or observations related to the culture.
    /// </summary>
    public string AdditionalNotes { get; set; }

    /// <summary>
    /// Optional comment field for user or system annotations.
    /// </summary>
    public string Comment1 { get; set; }

    /// <summary>
    /// Secondary comment field for extended annotations.
    /// </summary>
    public string Comment2 { get; set; }

    /// <summary>
    /// Barcode assigned by the manufacturer for tracking the culture bottle.
    /// </summary>
    public string ManufacturersBarcode { get; set; }

    /// <summary>
    /// Identifier for the specimen type (e.g., blood, urine).
    /// </summary>
    public string SpecimenTypeId { get; set; }

    /// <summary>
    /// Identifier for the laboratory where the culture was processed.
    /// </summary>
    public string LaboratoryId { get; set; }

    /// <summary>
    /// Internal identifier for the specimen. Defaults to 0 if not assigned.
    /// </summary>
    public int SpecimenId { get; set; } = 0;

    /// <summary>
    /// List of crafted models associated with this culture, such as derived data or interpretations.
    /// </summary>
    public List<CraftedModel> Crafted { get; set; }

    /// <summary>
    /// Weight of the culture bottle, typically used for quality control.
    /// </summary>
    public string CultureBottleWeight { get; set; }

    /// <summary>
    /// Combined weight of blood and bottle, used for sample integrity checks.
    /// </summary>
    public string CultureBloodAndBottleWeight { get; set; }

    /// <summary>
    /// Identification percentage, possibly representing confidence or match rate.
    /// </summary>
    public string IdPercentage { get; set; }

    /// <summary>
    /// Identifier for the growth setting for this culture.
    /// </summary>
    public string GrowthId { get; set; }

    /// <summary>
    /// Identifier for the parent culture for this isolate.
    /// </summary>
    public int ParentCultureId { get; set; }
}
