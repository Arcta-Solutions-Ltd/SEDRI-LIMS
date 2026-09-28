using arc.common.Models.AST;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Builds OrganismList pipe-delimited rows for specimen reports from AST query results.
/// </summary>
public static class OrganismListReportRowBuilder
{
    private const int MicTestMethodId = 680;

    /// <summary>Result of building OrganismList rows, including counts for support logging.</summary>
    public sealed class BuildResult
    {
        /// <summary>Pipe-delimited OrganismList rows in query order.</summary>
        public List<string> Rows { get; init; } = [];

        /// <summary>Rows included on the report.</summary>
        public int IncludedCount { get; init; }

        /// <summary>Rows excluded from the report.</summary>
        public int ExcludedCount { get; init; }

        /// <summary>Excluded rows for per-row support logging.</summary>
        public IReadOnlyList<AntibioticListForReportModel> ExcludedRows { get; init; } = [];
    }

    /// <summary>
    /// Builds report rows preserving query order (<c>testmethodid</c>, <c>id</c>).
    /// Only rows with a resolved susceptibility value are included.
    /// </summary>
    /// <param name="astResults">Rows from <see cref="AntibioticListForSpecimenReportQuery"/>.</param>
    /// <returns>OrganismList rows and inclusion counts.</returns>
    public static BuildResult BuildRows(IReadOnlyList<AntibioticListForReportModel> astResults)
    {
        if (astResults == null || astResults.Count == 0)
        {
            return new BuildResult();
        }

        var rows = new List<string>();
        var excludedRows = new List<AntibioticListForReportModel>();
        var includedCount = 0;

        foreach (var astResult in astResults)
        {
            if (!astResult.ShouldIncludeOnSpecimenReport())
            {
                excludedRows.Add(astResult);
                continue;
            }

            if (!astResult.HasSusceptibilityForReport())
            {
                excludedRows.Add(astResult);
                continue;
            }

            var rowText = FormatRow(astResult);
            rows.Add(rowText);
            includedCount++;
        }

        return new BuildResult
        {
            Rows = rows,
            IncludedCount = includedCount,
            ExcludedCount = excludedRows.Count,
            ExcludedRows = excludedRows
        };
    }

    /// <summary>
    /// Formats one OrganismList row: Antibiotic|Susceptibility|SpecialConsideration|MethodTag.
    /// </summary>
    /// <param name="source">AST row from the specimen report antibiotic list query.</param>
    /// <returns>Pipe-delimited row string for the OrganismList table.</returns>
    public static string FormatRow(AntibioticListForReportModel source)
    {
        var testMethod = source.TestMethodId == MicTestMethodId ? "@AstMicA@" : "@GenDisC@";
        var susceptibility = source.Susceptibility ?? string.Empty;
        var specialConsideration = source.SpecialConsideration ?? string.Empty;

        return source.Antibiotic + "|" + susceptibility + "|" + specialConsideration + "|" + testMethod;
    }
}
