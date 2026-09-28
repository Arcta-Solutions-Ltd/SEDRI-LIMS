namespace arc.common.ExtensionMethods;

/// <summary>
/// Helpers for interpreting save operation return values from event handlers and repositories.
/// </summary>
public static class SaveResultExtensions
{
    /// <summary>
    /// Returns true when a numeric record id indicates the save did not produce a record.
    /// </summary>
    /// <param name="recordId">The id returned from an add or special-add event handler.</param>
    /// <returns><see langword="true"/> when the id is zero or negative.</returns>
    public static bool IsFailedSaveId(this int recordId) => recordId <= 0;
}
