using System.Collections.Generic;

namespace arc.common.Models.Export
{
    /// <summary>
    /// Deserialisation model for a single node of the canonical export profile mapping tree
    /// (the <c>structure</c> column of <c>exportprofilemapping</c>). The tree is built by the
    /// Manage Mapping editor and consumed at export time by the JSON and XML format writers.
    /// </summary>
    /// <remarks>
    /// The canonical shape produced by the editor is:
    /// <list type="bullet">
    ///   <item><description><c>{ kind: 'object', name, children: [] }</c></description></item>
    ///   <item><description><c>{ kind: 'array', name, arrayType: 'specimens'|'cultures'|'grid'|'ast', gridFieldKey?, children: [] }</c></description></item>
    ///   <item><description><c>{ kind: 'attribute', name, fieldKey?, gridSubFieldId?, uniqueReference? }</c></description></item>
    /// </list>
    /// </remarks>
    public class MappingStructureNode
    {
        /// <summary>
        /// Gets or sets the node kind: <c>object</c>, <c>array</c> or <c>attribute</c>.
        /// </summary>
        public string Kind { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the output name for the node (JSON property name or XML element name).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the array type for <c>array</c> nodes: <c>specimens</c>, <c>cultures</c>,
        /// <c>grid</c> or <c>ast</c>. Null/empty for object and attribute nodes.
        /// </summary>
        public string ArrayType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the canonical key of the grid profile field a <c>grid</c> array is sourced from
        /// (format <c>{fieldId}|{formName}|{tableName}|{header}</c>). Null/empty for non-grid nodes.
        /// </summary>
        public string GridFieldKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the canonical key of the profile field an <c>attribute</c> leaf maps to
        /// (format <c>{fieldId}|{formName}|{tableName}|{header}</c>). The field id (first segment) is
        /// used for matching so translated headers never affect resolution.
        /// </summary>
        public string FieldKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the grid sub-field id an <c>attribute</c> leaf maps to when it lives inside a
        /// grid array (matches <c>FieldGridConfig.Id</c>). Null/empty for ordinary attributes.
        /// </summary>
        public string GridSubFieldId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether this attribute is the unique reference for its source-table bucket
        /// (<c>Yes</c> or <c>No</c>). At most one attribute per bucket (patient; specimen+tests;
        /// culture+culturetests; ast; custom) may be <c>Yes</c>. Export and load/unload semantics
        /// are defined separately.
        /// </summary>
        public string UniqueReference { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the child nodes for <c>object</c> and <c>array</c> nodes.
        /// </summary>
        public List<MappingStructureNode> Children { get; set; } = new();
    }
}
