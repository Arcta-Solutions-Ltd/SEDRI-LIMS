using arc.common.ExtensionMethods;
using arc.common.Models.AST;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.AST
{
    /// <summary>
    /// Susceptibility-gated duplicate antibiotic checks for flattened <see cref="ASTModel"/> save payloads.
    /// </summary>
    internal static class AstDuplicateChecker
    {
        /// <summary>
        /// Returns <c>@AstEmpB@</c> when another participating row shares antibiotic id and include-on-report flag
        /// anywhere in the payload (Disk, MIC, or expert-rule lines). Rows without a resolved susceptibility id are ignored.
        /// </summary>
        /// <param name="current">Row under validation.</param>
        /// <param name="allRows">Full flattened ASTResults list.</param>
        /// <returns>Language tag when duplicate; otherwise empty string.</returns>
        public static string CheckExactDuplicateRow(ASTModel current, IReadOnlyList<ASTModel> allRows)
        {
            if (!current.ParticipatesInDuplicateCheck() ||
                !current.TryGetAntibioticId(out var antibioticId))
            {
                return string.Empty;
            }

            var matchCount = allRows.Count(row =>
                row.ParticipatesInDuplicateCheck() &&
                row.TryGetAntibioticId(out var rowAntibioticId) &&
                rowAntibioticId == antibioticId &&
                row.IncludeInReport == current.IncludeInReport);

            return matchCount > 1 ? "@AstEmpB@" : string.Empty;
        }

        /// <summary>
        /// Tracks antibiotic ids seen globally among participating rows; returns <c>@AstDup@</c> on second occurrence
        /// anywhere in the payload (Disk, MIC, or expert-rule lines). Includes expert-rule lines when they have resolved susceptibility.
        /// </summary>
        /// <param name="current">Row under validation.</param>
        /// <param name="participatingAntibioticIds">Antibiotic ids already seen among participating rows.</param>
        /// <returns>Language tag when duplicate; otherwise empty string.</returns>
        public static string TrackGlobalDuplicate(
            ASTModel current,
            HashSet<int> participatingAntibioticIds)
        {
            if (!current.ParticipatesInDuplicateCheck() ||
                !current.TryGetAntibioticId(out var antibioticId))
            {
                return string.Empty;
            }

            if (participatingAntibioticIds.Contains(antibioticId))
            {
                return "@AstDup@";
            }

            participatingAntibioticIds.Add(antibioticId);
            return string.Empty;
        }

        /// <summary>
        /// Counts other participating rows that share antibiotic id with the current row (for logging).
        /// </summary>
        public static int CountAntibioticConflicts(ASTModel current, IReadOnlyList<ASTModel> allRows)
        {
            if (!current.ParticipatesInDuplicateCheck() ||
                !current.TryGetAntibioticId(out var antibioticId))
            {
                return 0;
            }

            return allRows.Count(row =>
                !ReferenceEquals(row, current) &&
                row.ParticipatesInDuplicateCheck() &&
                row.TryGetAntibioticId(out var rowAntibioticId) &&
                rowAntibioticId == antibioticId);
        }
    }
}
