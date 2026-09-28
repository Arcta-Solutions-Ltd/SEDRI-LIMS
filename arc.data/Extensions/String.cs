namespace arc.data.Extensions;
/// <summary>
/// Provides extension methods for string manipulation in SQL contexts.
/// </summary>
public static class SqlStringExtensions
{
    /// <summary>
    /// Wraps a string with SQL LIKE wildcards (%) for partial matching.
    /// </summary>
    /// <param name="value">The string to wrap with wildcards.</param>
    /// <returns>The string wrapped with % wildcards</returns>
    public static string ToSqlWildcard(this string value)
    {
        return $"%{value}%";
    }

    /// <summary>
    /// Wraps a string with SQL LIKE wildcards (%) at the end for "starts with" matching.
    /// </summary>
    /// <param name="value">The string to append a wildcard to.</param>
    /// <returns>The string with an appended % wildcard</returns>
    public static string ToSqlStartsWith(this string value)
    {
        return $"{value}%";
    }

    /// <summary>
    /// Wraps a string with SQL LIKE wildcards (%) at the start for "ends with" matching.
    /// </summary>
    /// <param name="value">The string to prepend a wildcard to.</param>
    /// <returns>The string with a prepended % wildcard</returns>
    public static string ToSqlEndsWith(this string value)
    {
        return $"%{value}";
    }
}
