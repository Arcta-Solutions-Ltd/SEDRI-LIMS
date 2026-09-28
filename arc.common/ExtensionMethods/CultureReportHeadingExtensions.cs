using System.Linq;
using arc.common.Models.Specimen;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Builds culture/isolate headings for specimen reports.
/// </summary>
public static class CultureReportHeadingExtensions
{
    /// <summary>
    /// Builds the culture/isolate heading for reports: optional culture number prefix,
    /// then culture type, growth, and quantity joined by " - ", omitting blank segments.
    /// When a specimen organism is present, appends the organism-of tag and organism name.
    /// </summary>
    /// <param name="cultureNumber">Culture sequence number; values greater than zero produce a "{n}. " prefix.</param>
    /// <param name="cultureType">Translated culture type display text.</param>
    /// <param name="growth">Translated growth display text.</param>
    /// <param name="quantity">Translated specimen quantity display text.</param>
    /// <param name="specimenOrganism">Optional isolated organism name.</param>
    /// <param name="organismOfTag">Language tag placed before the organism name (default @GenOfA@).</param>
    /// <returns>The formatted heading string.</returns>
    public static string BuildCultureReportHeading(
        int cultureNumber,
        string cultureType,
        string growth,
        string quantity,
        string specimenOrganism = null,
        string organismOfTag = "@GenOfA@")
    {
        var prefix = cultureNumber > 0 ? $"{cultureNumber}. " : string.Empty;
        var heading = prefix + string.Join(" - ", new[] { cultureType, growth, quantity }.Where(part => !string.IsNullOrWhiteSpace(part)));

        if (!string.IsNullOrWhiteSpace(specimenOrganism))
        {
            heading += $" {organismOfTag} {specimenOrganism}";
        }

        return heading;
    }

    /// <summary>
    /// Builds the report heading for a <see cref="CultureListModel"/> using its display fields.
    /// </summary>
    /// <param name="culture">Culture row with type, growth, quantity, and optional organism display values.</param>
    /// <param name="organismOfTag">Language tag placed before the organism name (default @GenOfA@).</param>
    /// <returns>The formatted heading string.</returns>
    public static string BuildCultureReportHeading(this CultureListModel culture, string organismOfTag = "@GenOfA@")
    {
        if (culture == null)
        {
            return string.Empty;
        }

        return BuildCultureReportHeading(
            culture.CultureNumber,
            culture.Type,
            culture.Growth,
            culture.SpecimenQuantity,
            culture.SpecimenOrganism,
            organismOfTag);
    }
}
