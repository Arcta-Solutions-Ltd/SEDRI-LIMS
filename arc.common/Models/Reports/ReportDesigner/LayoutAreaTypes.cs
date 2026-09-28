namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Stable type identifiers for the areas a layout section can place on a row.
/// </summary>
public static class LayoutAreaTypes
{
    /// <summary>
    /// The scalar field block, containing every field placed in the format's field columns.
    /// </summary>
    public const string Fields = "Fields";

    /// <summary>
    /// A single bound grid, identified by its stable data section grid id.
    /// </summary>
    public const string Grid = "Grid";

    /// <summary>
    /// Determines whether an area type identifier refers to the scalar field block.
    /// </summary>
    /// <param name="type">Area type identifier, in any casing.</param>
    /// <returns>True when the identifier is the field block type.</returns>
    public static bool IsFieldsArea(string type) =>
        string.Equals(type?.Trim(), Fields, System.StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Determines whether an area type identifier refers to a bound grid.
    /// </summary>
    /// <param name="type">Area type identifier, in any casing.</param>
    /// <returns>True when the identifier is the grid type.</returns>
    public static bool IsGridArea(string type) =>
        string.Equals(type?.Trim(), Grid, System.StringComparison.OrdinalIgnoreCase);
}
