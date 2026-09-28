namespace arc.domain.Configuration.EventsConfig
{
    /// <summary>
    /// Declares a nested collection or object inside an event payload that should be rendered as its
    /// own block, indented beneath its parent. Only fields that also appear in the event's
    /// <see cref="DisplayConfig"/> rows are shown inside a declared section; nested structures that
    /// are not declared as a section are dropped entirely.
    /// </summary>
    public class DisplaySectionConfig
    {
        /// <summary>
        /// Dot separated path to the nested structure relative to the display root, for example
        /// <c>ASTResults</c> or <c>ASTResults.SpecialRows</c>. Matched case-insensitively.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Language tag used as the heading for the nested block.
        /// </summary>
        public string Translation { get; set; }
    }
}
