using System.Collections.Generic;
using System.Linq;

namespace arc.common.Models.Reports
{
    /// <summary>
    /// Represents the model for a specimen selector list used to filter reports.
    /// </summary>
    public class SpecimenSelectorListModel
    {
        /// <summary>
        /// Gets or sets the ID of the specimen.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the list of direct tests to be included on a report for the specimen.
        /// </summary>
        public List<DirectTestSelectorListModel> DirectTestList { get; set; }

        /// <summary>
        /// Gets or sets the list of cultures to be included on a report for the specimen.
        /// </summary>
        public List<CultureSelectorListModel> Cultures { get; set; }

        /// <summary>
        /// Gets or sets the list of comments to be included on a report for the specimen.
        /// </summary>
        public List<CommentSelectorListModel> Comments { get; set; }

        /// <summary>
        /// Determines if a direct test should be included on the report based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the direct test.</param>
        /// <returns>True if the direct test is included; otherwise, false.</returns>
        public bool IncludeDirectTestOnReport(string id)
        {
            return DirectTestList.Any(d => d.Id == id && d.PrintOnReport == "Yes");
        }

        /// <summary>
        /// Determines if a culture should be included on the report based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the culture.</param>
        /// <returns>True if the culture is included; otherwise, false.</returns>
        public bool IncludeCultureOnReport(string id)
        {
            return Cultures.Any(d => d.Id == id && d.PrintOnReport == "Yes");
        }

        /// <summary>
        /// Determines if comments should be included on the report based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <returns>True if the comment is included; otherwise, false.</returns>
        public bool IncludeCommentOnReport(string id)
        {
            return Comments.Any(d => d.Id == id && d.PrintOnReport == "Yes");
        }

    }

}
