namespace arc.data.model.Quality;

/// <summary>
/// Represents the fields in the iqctest table in the database.
/// </summary>
public class IqcTestDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the accession number for the IQC test.
    /// </summary>
    public string? AccessionNumber { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the iqctest table. This links to the state list in the listitem table.
    /// </summary>
    public int StateId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the iqctestprofile table to the iqctest table.
    /// </summary>
    public int IqcTestProfileId { get; set; }

    /// <summary>
    /// Gets or sets comments for the IQC test.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Gets or sets the date the IQC test was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the date the IQC test was completed.
    /// </summary>
    public DateTime? CompletedDate { get; set; }
}
