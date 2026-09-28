namespace arc.data.model.Specimen;

/// <summary>
/// Represents the fields in the barcodecounter table in the database.
/// </summary>
public class BarcodeCounterDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the barcode count value.
    /// </summary>
    public string? BarcodeCount { get; set; }
}
