namespace arc.common.Models.Tests;

/// <summary>
/// Result of find-or-insert queries for Tests / CultureTests rows (manual request prerequisites and related flows).
/// </summary>
public class EnsureTestRowResult
{
    public int Id { get; set; }
    /// <summary>True when a new row was inserted; false when an existing row was returned.</summary>
    public bool WasCreated { get; set; }
}
