namespace arc.data.model.Quality;
/// <summary>
/// Data model for QC Antibiotics.
/// </summary>
internal class QcAntibioticsDataModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the organism.
    /// </summary>
    public int QcOrganismId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the antibiotic.
    /// </summary>
    public int AntibioticsId { get; set; }

    /// <summary>
    /// Gets or sets the minimum value of the MIC target.
    /// </summary>
    public long MicTargetLower { get; set; }

    /// <summary>
    /// Gets or sets the maximum value of the MIC target.
    /// </summary>
    public long MicTargetUpper { get; set; }

    /// <summary>
    /// Gets or sets the lower bound of the MIC range.
    /// </summary>
    public long MicRangeLower { get; set; }

    /// <summary>
    /// Gets or sets the upper bound of the MIC range.
    /// </summary>
    public long MicRangeUpper { get; set; }

    /// <summary>
    /// Gets or sets the content of the antibiotic disk, can be null.
    /// </summary>
    public string? DiskContent { get; set; }

    /// <summary>
    /// Gets or sets the lower bound of the inhibition zone diameter target.
    /// </summary>
    public long InhibitionZoneDiameterTargetLower { get; set; }

    /// <summary>
    /// Gets or sets the upper bound of the inhibition zone diameter target.
    /// </summary>
    public long InhibitionZoneDiameterTargetUpper { get; set; }

    /// <summary>
    /// Gets or sets the lower bound of the inhibition zone diameter range.
    /// </summary>
    public long InhibitionZoneDiameterRangeLower { get; set; }

    /// <summary>
    /// Gets or sets the upper bound of the inhibition zone diameter range.
    /// </summary>
    public long InhibitionZoneDiameterRangeUpper { get; set; }
}
