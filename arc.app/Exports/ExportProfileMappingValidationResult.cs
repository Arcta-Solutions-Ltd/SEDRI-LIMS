namespace arc.app.Exports
{
    /// <summary>
    /// Outcome of validating an export profile mapping payload. When validation succeeds the
    /// mapping is safe to persist; the counts describe how many nodes were intentionally left
    /// unmapped (an attribute with no field binding, or an array with no type). Such nodes are
    /// permitted and are ignored by the load/unload process, so the counts are surfaced purely
    /// for diagnostics/logging on installed systems.
    /// </summary>
    public class ExportProfileMappingValidationResult
    {
        /// <summary>
        /// Number of attribute nodes saved without a <c>fieldKey</c> or <c>gridSubFieldId</c>
        /// (unmapped attributes that will be ignored downstream).
        /// </summary>
        public int UnmappedAttributeCount { get; set; }

        /// <summary>
        /// Number of array nodes saved without an <c>arrayType</c> (untyped arrays that,
        /// together with their subtree, will be ignored downstream).
        /// </summary>
        public int UntypedArrayCount { get; set; }
    }
}
