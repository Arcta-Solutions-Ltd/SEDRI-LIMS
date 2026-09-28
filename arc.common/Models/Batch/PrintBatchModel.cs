using System.Collections.Generic;

namespace arc.common.Models.Batch;

/// <summary>
/// Represents the data required for a batch print operation, including report configuration and item selection.
/// </summary>
public class PrintBatchModel
{
    /// <summary>
    /// The identifier of the report to be printed.
    /// </summary>
    public int ReportId { get; set; }

    /// <summary>
    /// The type of operation to perform, such as "print" or other supported actions.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// A list of item identifiers to be included in the batch print.
    /// </summary>
    public List<string> ItemsToPrint { get; set; }

    /// <summary>
    /// Indicates whether the operation should retrieve historical report data.
    /// </summary>
    public bool History { get; set; } = false;
}
