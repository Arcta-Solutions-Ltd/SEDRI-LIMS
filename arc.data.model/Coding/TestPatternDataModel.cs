namespace arc.data.model.Coding
{
    /// <summary>
    /// Represents the fields in the testpattern table in the database.
    /// </summary>
    public class TestPatternDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets the value of the test pattern name.
        /// </summary>
        public required string TestPatternName { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the order table to the testpattern table.
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the family table to the testpattern table.
        /// </summary>
        public int FamilyId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the organism table to the testpattern table.
        /// </summary>
        public int OrganismId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the testpattern table. This links to the organism group list in the listitem table.
        /// </summary>
        public int OrgGroupCodingId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the testpattern table. This links to the host list (typically Human or Animal), in the listitem table.
        /// </summary>
        public int HostId { get; set; }
        /// <summary>
        /// Gets or sets the value of the MakeDefault flag ('Yes' or 'No'), for the test pattern. When more than one test pattern matches the criteria for selection when calculating susceptibilities the one where MakeDefault is 'Yes' will be used.
        /// </summary>
        public string? MakeDefault { get; set; }
    }
}
