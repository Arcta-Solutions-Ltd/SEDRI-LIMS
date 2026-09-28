namespace arc.domain.Instruments
{
    /// <summary>
    /// One interface criteria row for a Custom interface profile (<see cref="SingleInstrumentConfig.InterfaceCriteria"/>).
    /// <see cref="Field"/> holds the export profile field id (stable id, not the translated label). The comparison
    /// value is stored in whichever of <see cref="StringValue"/>, <see cref="NumberValue"/> or <see cref="ListValue"/>
    /// applies to the field type; <see cref="ListValue"/> holds a list item id so matching is language independent.
    /// </summary>
    public class InterfaceCriteriaLine
    {
        /// <summary>Export profile field id (exportprofilerecord.id) the criteria applies to.</summary>
        public string Field { get; set; }
        /// <summary>Comparison operator: =, &gt;, &gt;=, &lt;= or &lt;.</summary>
        public string Comparison { get; set; }
        /// <summary>Comparison value for free-text fields.</summary>
        public string StringValue { get; set; }
        /// <summary>Comparison value for numeric fields.</summary>
        public string NumberValue { get; set; }
        /// <summary>Comparison value for list/combobox fields, stored as a list item id.</summary>
        public string ListValue { get; set; }
    }
}
