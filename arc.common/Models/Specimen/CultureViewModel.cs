namespace arc.common.Models.Specimen;

/// <summary>
/// Represents detailed information about a specimen culture, including organism identification,
/// antimicrobial susceptibility testing (AST), and reporting metadata.
/// </summary>
public class CultureViewModel
{
    /// <summary>Unique identifier for the culture entry.</summary>
    public string Id { get; set; }

    /// <summary>Type of culture specimen (e.g., blood, urine, swab).</summary>
    public string Type { get; set; }

    /// <summary>Organism identified in the specimen.</summary>
    public string SpecimenOrganism { get; set; }

    /// <summary>Classification group of the organism (e.g., Gram-positive).</summary>
    public string OrgGroup { get; set; }

    /// <summary>Reported quantity or concentration of the specimen.</summary>
    public string SpecimenQuantity { get; set; }

    /// <summary>Barcode assigned by the manufacturer for tracking purposes.</summary>
    public string ManufacturersBarcode { get; set; }

    /// <summary>Date when the specimen tested positive.</summary>
    public string PositiveDate { get; set; }

    /// <summary>Time when the specimen tested positive.</summary>
    public string PositiveTime { get; set; }

    /// <summary>API panel identifier used for organism identification.</summary>
    public string SpecimenApiIdPanel { get; set; }

    /// <summary>Indicates whether ESBL (Extended Spectrum Beta-Lactamase) was detected.</summary>
    public string Esbl { get; set; }

    /// <summary>Flag indicating whether this culture should be shown on the final report.</summary>
    public string DisplayOnReport { get; set; }

    /// <summary>Profile ID used for organism identification matching.</summary>
    public string IdProfile { get; set; }

    /// <summary>Confidence percentage of the organism identification.</summary>
    public string IDPercentage { get; set; }

    /// <summary>Identifier for the aliquot (subsample) used in testing.</summary>
    public string AloquatId { get; set; }

    /// <summary>General notes related to the culture specimen.</summary>
    public string AdditionalNotes { get; set; }

    /// <summary>Additional notes specific to antimicrobial susceptibility testing (AST).</summary>
    public string ASTAdditionalNotes { get; set; }

    /// <summary>First general comment related to the culture.</summary>
    public string CommentOne { get; set; }

    /// <summary>Second general comment related to the culture.</summary>
    public string CommentTwo { get; set; }

    /// <summary>First comment specific to AST results.</summary>
    public string ASTCommentOne { get; set; }

    /// <summary>Second comment specific to AST results.</summary>
    public string ASTCommentTwo { get; set; }

    /// <summary>Weight of the culture bottle alone.</summary>
    public string CultureBottleWeight { get; set; }

    /// <summary>Total weight of blood and culture bottle combined.</summary>
    public string CultureBloodAndBottleWeight { get; set; }

    /// <summary>Indicates whether microbial growth was observed.</summary>
    public string Growth { get; set; }

    public string MoreData { get; set; }
    public int SpecimenTypeId { get; set; }

    /// <summary>Comma-separated file attachment IDs for the culture.</summary>
    public string FileAttachmentIds { get; set; }
}