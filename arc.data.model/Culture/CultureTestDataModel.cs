namespace arc.data.model.Culture;

/// <summary>
/// Represents the fields in the culturetest table in the database.
/// </summary>
public class CultureTestDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the culture table to the culturetest table.
    /// </summary>
    public int? CultureId { get; set; }

    /// <summary>
    /// Gets or sets the name of the test.
    /// </summary>
    public required string TestName { get; set; }

    /// <summary>
    /// Gets or sets the test results.
    /// </summary>
    public string? TestResults { get; set; }

    /// <summary>
    /// Gets or sets the status of the test.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Gets or sets the date the test was requested.
    /// </summary>
    public DateTime Requested { get; set; }

    /// <summary>
    /// Gets or sets the date the test was completed.
    /// </summary>
    public DateTime? Completed { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the alerttype table to the culturetest table.
    /// </summary>
    public int? AlertTypeId { get; set; }
}
