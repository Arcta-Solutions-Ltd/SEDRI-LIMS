namespace arc.common.Models.Specimen;

/// <summary>
/// Represents the set of pages used to acknowledge receipt of a specimen.
/// Inherits shared page properties and behavior from JustCraftedPages.
/// </summary>
public class AcknowledgeReceiptCraftedPages : JustCraftedPages
{
    /// <summary>
    /// Gets or sets the primary key for this acknowledge‐receipt entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the specimen type being acknowledged.
    /// This typically maps to a code in the specimen‐type registry.
    /// </summary>
    public string SpecimenTypeId { get; set; }

    /// <summary>
    /// Gets or sets the laboratory identifier where the specimen was received.
    /// </summary>
    public int LaboratoryId { get; set; }

    /// <summary>
    /// Gets or sets a flag or configuration key indicating whether default values
    /// should be applied when rendering the acknowledgement pages.
    /// </summary>
    public string ApplyDefaults { get; set; }
}
