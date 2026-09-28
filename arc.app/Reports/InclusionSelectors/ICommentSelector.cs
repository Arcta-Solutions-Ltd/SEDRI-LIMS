using arc.common.Models.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Reports.InclusionSelectors
{
    /// <summary>
    /// Interface for selecting comments used in reporting.
    /// </summary>
    public interface ICommentSelector
    {
        /// <summary>
        /// Asynchronously gets comments for a given specimen.
        /// </summary>
        /// <param name="specimenId">The ID of the specimen.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of CommentSelectorListModel objects.</returns>
        Task<List<CommentSelectorListModel>> GetCommentsAsync(int specimenId);
    }
}
