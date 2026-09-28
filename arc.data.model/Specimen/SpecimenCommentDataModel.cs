namespace arc.data.model.Specimen;

/// <summary>
/// Represents the fields in the specimencomment table in the database.
/// </summary>
public class SpecimenCommentDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the specimen table to the specimencomment table.
    /// </summary>
    public int? SpecimenId { get; set; }

    /// <summary>
    /// Gets or sets the comment text.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specimencomment table. This links to the commenttype list in the listitem table.
    /// </summary>
    public int? CommentTypeId { get; set; }

    /// <summary>
    /// Gets or sets whether the comment should be displayed on the report.
    /// </summary>
    public string? DisplayOnReport { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the culture table to the specimencomment table.
    /// </summary>
    public int? CultureId { get; set; }

    /// <summary>
    /// Gets or sets the date the comment was added.
    /// </summary>
    public DateOnly? AddedDate { get; set; }

    /// <summary>
    /// Gets or sets the user who added the comment.
    /// </summary>
    public string? AddedBy { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specimencomment table. This links to the cannedcomment list in the listitem table.
    /// </summary>
    public int? CannedCommentId { get; set; }

    /// <summary>
    /// Gets or sets the order of the comment.
    /// </summary>
    public int? CommentOrder { get; set; }

    /// <summary>
    /// Gets or sets the field ID for the comment.
    /// </summary>
    public string? FieldId { get; set; }
}
