using arc.app.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;
using arc.common.Models.Reports;

namespace arc.app.Reports.InclusionSelectors
{
    /// <summary>
    /// Report inclusion selector for comments.
    /// </summary>
    public class CommentSelector : ICommentSelector
    {
        private readonly ICommentRepository _commentRepository;

        /// <summary>
        /// Initializes a new instance of the CommentSelector class.
        /// </summary>
        /// <param name="commentRepository">The comment repository instance.</param>
        public CommentSelector(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        /// <summary>
        /// Asynchronously gets comments for a given specimen.
        /// </summary>
        /// <param name="specimenId">The ID of the specimen.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of CommentSelectorListModel objects.</returns>
        public async Task<List<CommentSelectorListModel>> GetCommentsAsync(int specimenId)
        {
            var queryFilters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "specimenid", Value = specimenId.ToString() } } };
            var comments = await _commentRepository.GetSpecimenCommentListByIdAsync(queryFilters);

            var returnList = new List<CommentSelectorListModel>();
            foreach (var comment in comments)
            {
                var newModel = new CommentSelectorListModel
                {
                    Id = comment.Id,
                    Comment = comment.Comment,
                    PrintOnReport = comment.DisplayOnReport,
                    CommentType = comment.CommentType,
                    Username = comment.AddedBy
                };
                returnList.Add(newModel);
            }

            return returnList;
        }
    }
}
