using arc.common.Models.AST;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Report inclusion rules for AST rows returned by <see cref="AntibioticListForSpecimenReportQuery"/>.
/// </summary>
public static class AntibioticListForReportExtensions
{
    /// <summary>List item id meaning no special consideration on a parent AST row.</summary>
    public const int NoSpecialConsiderationSentinelId = 973;

    /// <summary>
    /// Whether an AST row should appear on the specimen report OrganismList table.
    /// Parent rows use <see cref="AntibioticListForReportModel.DisplayOnReport"/> (<c>AST.displayonreport</c>).
    /// Special consideration rows use <see cref="AntibioticListForReportModel.SpecialDisplayOnReport"/>
    /// (<c>specialastrow.displayonreport</c>). Row type is determined by special consideration id, not translated labels.
    /// Rows must also have a resolved susceptibility value; see <see cref="HasSusceptibilityForReport"/>.
    /// </summary>
    /// <param name="row">AST row from the specimen report antibiotic list query.</param>
    /// <returns><c>true</c> when the row should be included on the report.</returns>
    public static bool ShouldIncludeOnSpecimenReport(this AntibioticListForReportModel row)
    {
        if (row == null)
        {
            return false;
        }

        if (IsSpecialConsiderationRow(row))
        {
            return row.SpecialDisplayOnReport == "Yes";
        }

        return row.DisplayOnReport == "Yes";
    }

    /// <summary>
    /// Whether the row has a non-blank susceptibility value for specimen report inclusion.
    /// Parent and special-consideration rows without susceptibility must not print on the report.
    /// </summary>
    /// <param name="row">AST row from the specimen report antibiotic list query.</param>
    /// <returns><c>true</c> when <see cref="AntibioticListForReportModel.Susceptibility"/> is non-empty.</returns>
    public static bool HasSusceptibilityForReport(this AntibioticListForReportModel row)
    {
        return row != null && !string.IsNullOrWhiteSpace(row.Susceptibility);
    }

    /// <summary>
    /// Resolves the display-on-report flag used for report inclusion logging.
    /// </summary>
    /// <param name="row">AST row from the specimen report antibiotic list query.</param>
    /// <returns>The effective Yes/No flag evaluated for report inclusion.</returns>
    public static string ResolveDisplayOnReportForReport(this AntibioticListForReportModel row)
    {
        if (row == null)
        {
            return "No";
        }

        return IsSpecialConsiderationRow(row) ? row.SpecialDisplayOnReport : row.DisplayOnReport;
    }

    /// <summary>
    /// Whether the special consideration id denotes a special-consideration AST embed row (not a parent line).
    /// </summary>
    /// <param name="specialConsiderationId">Special type list item id from query or culture print selector payload.</param>
    /// <returns><c>true</c> when the id targets <c>specialastrow</c> rather than parent <c>AST</c>.</returns>
    public static bool IsSpecialConsiderationReportRow(int specialConsiderationId)
    {
        return specialConsiderationId != 0 && specialConsiderationId != NoSpecialConsiderationSentinelId;
    }

    private static bool IsSpecialConsiderationRow(AntibioticListForReportModel row)
    {
        return row.SpecialConsiderationId != 0 && row.SpecialConsiderationId != NoSpecialConsiderationSentinelId;
    }
}
