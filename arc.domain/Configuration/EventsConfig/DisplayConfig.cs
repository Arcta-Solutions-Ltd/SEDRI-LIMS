namespace arc.domain.Configuration.EventsConfig
{
    /// <summary>
    /// One row of an event's display configuration, describing how a single stored field is
    /// labelled and resolved when an event payload is rendered for a human (diary entry,
    /// monitoring event details, test list callouts).
    /// </summary>
    public class DisplayConfig
    {
        /// <summary>
        /// Field id as it appears in the stored event payload. Matched case-insensitively.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Language tag (for example <c>@GenAnt@</c>) substituted for <see cref="Label"/> before the
        /// payload is returned, then replaced with the user's language by the language handler.
        /// </summary>
        public string Translation { get; set; }

        /// <summary>
        /// When <c>"Yes"</c>, the stored value is treated as one or more comma separated
        /// <c>ListItem</c> ids and resolved to their values.
        /// </summary>
        public string List { get; set; }

        /// <summary>
        /// When true, the stored value is trimmed to its leading <c>yyyy-MM-dd</c> date portion.
        /// </summary>
        public bool Date { get; set; }

        /// <summary>
        /// Optional name of a registered display value resolver used to turn a stored id into display
        /// text when the id does not come from <c>ListItem</c> (for example <c>antibiotic</c>).
        /// Resolution is always keyed on the stored id, never on a display value, so it survives
        /// translation of tags and list items.
        /// </summary>
        public string Resolver { get; set; }

        /// <summary>
        /// When true, this field holds a collection that is rendered as a grid with a column heading
        /// row built from the display rows of its child fields.
        /// </summary>
        public bool Grid { get; set; }
    }
}
