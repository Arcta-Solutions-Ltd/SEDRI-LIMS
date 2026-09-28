using System.Collections.Generic;

namespace arc.app.Exports.Formatters
{
    /// <summary>
    /// The kind of an <see cref="ExportNode"/> in the intermediate document tree.
    /// </summary>
    public enum ExportNodeType
    {
        /// <summary>A named object with child nodes.</summary>
        Object,

        /// <summary>A named array whose children are the repeated items.</summary>
        Array,

        /// <summary>A named leaf carrying a single string value.</summary>
        Value
    }

    /// <summary>
    /// Format-agnostic intermediate representation of an export document. Built once by
    /// <see cref="ExportDocumentBuilder"/> from the mapping tree and the row data, then serialised
    /// by both the JSON and XML writers (XML being "just serialising the json").
    /// </summary>
    public class ExportNode
    {
        /// <summary>
        /// Gets or sets the node name (JSON property / XML element name).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the node type.
        /// </summary>
        public ExportNodeType Type { get; set; }

        /// <summary>
        /// Gets or sets the scalar value for <see cref="ExportNodeType.Value"/> nodes.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the child nodes for object and array nodes.
        /// </summary>
        public List<ExportNode> Children { get; set; } = new();
    }
}
