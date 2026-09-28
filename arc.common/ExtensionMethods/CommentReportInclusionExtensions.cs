using arc.common.Models.Reports;
using arc.common.Models.Specimen;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Report inclusion rules for specimen and culture comments on specimen record reports.
/// </summary>
public static class CommentReportInclusionExtensions
{
    /// <summary>
    /// Determines whether a comment should appear on a specimen report.
    /// Specimen-level comments honour <see cref="SpecimenSelectorListModel"/> amendments when present.
    /// Culture/isolate comments always use persisted <see cref="CommentModel.DisplayOnReport"/>
    /// (saved immediately via culture print selector), matching AST and culture-test sub-form behaviour.
    /// </summary>
    /// <param name="comment">Comment row from the report comments query.</param>
    /// <param name="reportCriteria">In-memory report filter amendments from the approval form, or null.</param>
    /// <returns><c>true</c> when the comment should be included on the report.</returns>
    public static bool ShouldIncludeOnSpecimenReport(this CommentModel comment, SpecimenSelectorListModel reportCriteria)
    {
        if (comment == null)
        {
            return false;
        }

        if (reportCriteria == null)
        {
            return comment.DisplayOnReport == "Yes";
        }

        if (comment.IsCultureComment())
        {
            return comment.DisplayOnReport == "Yes";
        }

        return reportCriteria.IncludeCommentOnReport(comment.Id.ToString());
    }
}
