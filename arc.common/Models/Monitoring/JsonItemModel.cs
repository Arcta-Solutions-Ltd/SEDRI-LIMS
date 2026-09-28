using System.Collections.Generic;

namespace arc.common.Models.Monitoring
{
    /// <summary>
    /// One rendered field from a stored event payload. Scalars carry <see cref="Contents"/>; a nested
    /// object carries <see cref="ChildItems"/>; a nested collection carries <see cref="ArrayItems"/>.
    /// </summary>
    public class JsonItemModel
    {
        /// <summary>
        /// Untranslated field id from the stored payload. <see cref="Label"/> is replaced with a language
        /// tag before the payload is returned, so this is the only stable handle the front end can use
        /// for element ids and the only value safe to match on across languages.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Display label. Starts as the payload field id and is replaced with the configured language
        /// tag, which the language handler then substitutes for the user's language.
        /// </summary>
        public string Label { get; set; }

        public string Contents { get; set; }

        /// <summary>
        /// Column headings for a grid field, in the same order as each row's child items. Null unless the
        /// field's display row is marked as a grid.
        /// </summary>
        public List<string> ColumnHeadings { get; set; }

        public List<JsonArrayModel> ArrayItems { get; set; }
        public List<JsonItemModel> ChildItems { get; set; }
    }
}
