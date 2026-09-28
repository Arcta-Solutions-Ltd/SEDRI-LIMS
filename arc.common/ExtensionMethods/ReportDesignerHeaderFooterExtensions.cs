using arc.common.Models.Reports.ReportDesigner;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Helpers for header and footer sections in the report designer save path.
/// </summary>
public static class ReportDesignerHeaderFooterExtensions
{
    /// <summary>
    /// Determines whether a scope value identifies a header or footer section.
    /// </summary>
    /// <param name="scope">The section scope from the designer.</param>
    /// <returns>True when the scope is <see cref="ConfigScope.Header"/> or <see cref="ConfigScope.Footer"/>.</returns>
    public static bool IsHeaderOrFooterScope(this string scope)
    {
        return scope.IsSameConfigName(ConfigScope.Header)
               || scope.IsSameConfigName(ConfigScope.Footer);
    }

    /// <summary>
    /// Determines whether a section save should fork rather than update the shared record.
    /// </summary>
    /// <param name="section">The section being saved.</param>
    /// <returns>True when the section is a header or footer and save scope is current-report-only.</returns>
    public static bool RequiresFork(this ReportSectionModel section)
    {
        return section != null
               && section.Scope.IsHeaderOrFooterScope()
               && section.SaveScope.IsSameConfigName(HeaderFooterSaveScope.CurrentReportOnly);
    }

    /// <summary>
    /// Maps a header or footer scope to the report property that holds its name reference.
    /// </summary>
    /// <param name="scope">The section scope from the designer.</param>
    /// <returns><c>Header</c> or <c>Footer</c>, or null when the scope is not a header or footer.</returns>
    public static string ResolveHeaderFooterPropertyName(this string scope)
    {
        if (scope.IsSameConfigName(ConfigScope.Header))
        {
            return "Header";
        }

        if (scope.IsSameConfigName(ConfigScope.Footer))
        {
            return "Footer";
        }

        return null;
    }
}
