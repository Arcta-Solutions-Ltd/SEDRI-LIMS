namespace arc.common.Models.Specimen;

/// <summary>
/// Represents a detailed data model for a culture record, 
/// including organism data, growth metrics, comments, and reporting flags.
/// </summary>
public class CultureListModel
{
    /// <summary>
    /// Unique identifier of the culture record.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Name of the organism isolated from the specimen.
    /// </summary>
    public string SpecimenOrganism { get; set; }

    /// <summary>
    /// Combined culture/isolate report heading: culture type, growth, quantity, and optional organism suffix.
    /// </summary>
    public string OrganismWithGrowth { get; set; }

    /// <summary>
    /// Numeric identifier for the specimen organism.
    /// </summary>
    public int SpecimenOrganismId { get; set; }

    /// <summary>
    /// Identifier for the alert category associated with this culture.
    /// </summary>
    public int AlertCategoryId { get; set; }

    /// <summary>
    /// The organism’s group classification.
    /// </summary>
    public string OrgGroup { get; set; }

    /// <summary>
    /// Description of the culture type.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Identifier for the culture type.
    /// </summary>
    public string TypeId { get; set; }

    /// <summary>
    /// Textual description of the specimen quantity.
    /// </summary>
    public string SpecimenQuantity { get; set; }

    /// <summary>
    /// Identifier for the specimen quantity.
    /// </summary>
    public string SpecimenQuantityId { get; set; }

    /// <summary>
    /// Description of the growth condition or rate.
    /// </summary>
    public string Growth { get; set; }

    /// <summary>
    /// Identifier for the growth condition.
    /// </summary>
    public string GrowthId { get; set; }

    /// <summary>
    /// Parent list item id for the growth (e.g. 427=Growth, 428=No Growth).
    /// Used by rules to determine organism selection visibility without enumerating individual growth IDs.
    /// </summary>
    public string GrowthTypeParentId { get; set; }

    /// <summary>
    /// Date when the culture tested positive (formatted as string).
    /// </summary>
    public string PositiveDate { get; set; }

    /// <summary>
    /// Results of any tests performed on the culture.
    /// </summary>
    public string TestResults { get; set; }

    /// <summary>
    /// Identifier for the current workflow state of the culture.
    /// </summary>
    public string StateId { get; set; }

    /// <summary>
    /// Flag indicating whether this culture should be displayed on the final report.
    /// </summary>
    public string DisplayOnReport { get; set; }

    /// <summary>
    /// Colour code or description for display purposes.
    /// </summary>
    public string Colour { get; set; }

    /// <summary>
    /// First comment from the antimicrobial susceptibility test.
    /// </summary>
    public string ASTCommentOne { get; set; }

    /// <summary>
    /// Second comment from the antimicrobial susceptibility test.
    /// </summary>
    public string ASTCommentTwo { get; set; }

    /// <summary>
    /// Additional notes related to antimicrobial susceptibility testing.
    /// </summary>
    public string AstAdditionalNotes { get; set; }

    /// <summary>
    /// First general comment about the culture.
    /// </summary>
    public string CommentOne { get; set; }

    /// <summary>
    /// Second general comment about the culture.
    /// </summary>
    public string CommentTwo { get; set; }

    /// <summary>
    /// Sequential number assigned to this culture instance.
    /// </summary>
    public int CultureNumber { get; set; }

    /// <summary>
    /// Weight of the culture bottle alone.
    /// </summary>
    public string CultureBottleWeight { get; set; }

    /// <summary>
    /// Combined weight of the blood and bottle.
    /// </summary>
    public string CultureBloodAndBottleWeight { get; set; }

    /// <summary>
    /// Praent culture for an isolate.
    /// </summary>
    public string ParentCultureId { get; set; }

    public int SpecimenTypeId { get; set; }
}
