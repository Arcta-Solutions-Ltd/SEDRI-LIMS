namespace arc.data.model.Quality;

/// <summary>
/// Represents the fields in the iqcresult table in the database.
/// </summary>
public class IqcResultDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the iqctest table to the iqcresult table.
    /// </summary>
    public int IqcTestId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the qcantibiotic table to the iqcresult table.
    /// </summary>
    public int QcAntibioticId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the alerttype table to the iqcresult table.
    /// </summary>
    public int? AlertTypeId { get; set; }

    /// <summary>
    /// Gets or sets the value of the IQC result.
    /// </summary>
    public decimal? Value { get; set; }
}
