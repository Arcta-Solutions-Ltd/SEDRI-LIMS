namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the specification table in the database.
/// Stores specification definitions linking guidelines, document type, version number, source, and publication year.
/// </summary>
public class SpecificationDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specification table.
    /// Links to the guidelines list.
    /// </summary>
    public int GuidelinesId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specification table.
    /// Links to the document type list.
    /// </summary>
    public int DocumentId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specification table.
    /// Links to the version number list.
    /// </summary>
    public int? VersionNumberId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specification table.
    /// Links to the source list.
    /// </summary>
    public int? SourceId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specification table.
    /// Links to the publication year list.
    /// </summary>
    public int? PublicationYearId { get; set; }
}
