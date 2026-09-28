namespace arc.common.Models.Reports
{
    /// <summary>
    /// Represents the model for a comment selector list.
    /// </summary>
    public class CommentSelectorListModel
    {
        /// <summary>
        /// Gets or sets the ID of the comment.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating if the comment should be printed on the report.
        /// </summary>
        public string PrintOnReport { get; set; }

        /// <summary>
        /// Gets or sets the type of the comment.
        /// </summary>
        public string CommentType { get; set; }

        /// <summary>
        /// Gets or sets the username of the person who added the comment.
        /// </summary>
        public string Username { get; set; }
    }
}
