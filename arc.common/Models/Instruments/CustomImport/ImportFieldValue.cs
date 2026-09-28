namespace arc.common.Models.Instruments.CustomImport
{
    /// <summary>
    /// A single value extracted from an inbound Custom-interface file, bound to the export profile field that
    /// describes it. This is the reverse of an export attribute leaf: the mapping attribute's <c>fieldKey</c>
    /// resolves to the export profile field, which supplies <see cref="FieldName"/>, <see cref="FormName"/> and
    /// <see cref="TableName"/>.
    /// </summary>
    public class ImportFieldValue
    {
        /// <summary>Export profile field id (<c>exportprofilerecord.id</c>) this value was bound to.</summary>
        public int FieldId { get; set; }

        /// <summary>Source form field id / column source (e.g. <c>FirstName</c>, <c>AccessionNumber</c>).</summary>
        public string FieldName { get; set; }

        /// <summary>Source form name for the field (used to resolve list/tag fields to ids).</summary>
        public string FormName { get; set; }

        /// <summary>Raw export profile table name for the field (e.g. <c>patient</c>, <c>specimen</c>, <c>ast</c>).</summary>
        public string TableName { get; set; }

        /// <summary>Normalized unique-reference bucket for the field (patient; specimen; culture; ast).</summary>
        public string Bucket { get; set; }

        /// <summary>Output attribute name the value was read from in the JSON/XML document.</summary>
        public string OutputName { get; set; }

        /// <summary>Raw imported value as it appeared in the file (may be display text or an id).</summary>
        public string Value { get; set; }

        /// <summary>True when this attribute is the unique reference for its bucket in the profile mapping.</summary>
        public bool IsUniqueReference { get; set; }
    }
}
