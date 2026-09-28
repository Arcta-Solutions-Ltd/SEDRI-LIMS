namespace arc.data.model.Culture
{
    /// <summary>
    /// Represents the fields in the culture table in the database.
    /// </summary>
    public class CultureDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the culture table. This links to the culturetype list in the listitem table.
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the specimen table to the culture table.
        /// </summary>
        public int SpecimenId { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the organism table to the culture table.
        /// </summary>
        public int? SpecimenOrganismId { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the culture table. This links to the organism group coding list in the listitem table.
        /// </summary>
        public int? OrgGroupCodingId { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the culture table. This links to the quantity list in the listitem table.
        /// </summary>
        public int? SpecimenQuantityId { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the culture table. This links to the growth list in the listitem table.
        /// </summary>
        public int? GrowthId { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the API panel to the culture table.
        /// </summary>
        public int? SpecimenApiIdPanelId { get; set; }

        /// <summary>
        /// Gets or sets the ID profile identifier.
        /// </summary>
        public string? IdProfile { get; set; }

        /// <summary>
        /// Gets or sets the ID percentage value.
        /// </summary>
        public string? IdPercentage { get; set; }

        /// <summary>
        /// Gets or sets the date the culture became positive.
        /// </summary>
        public DateOnly? PositiveDate {  get; set; }

        /// <summary>
        /// Gets or sets the time the culture became positive.
        /// </summary>
        public string? PositiveTime { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the culture table. This links to the comment list in the listitem table.
        /// </summary>
        public int? CommentOneId {  get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the culture table. This links to the comment list in the listitem table.
        /// </summary>
        public int? CommentTwoId { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the culture table. This links to the AST comment list in the listitem table.
        /// </summary>
        public int? AstCommentOneId { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the culture table. This links to the AST comment list in the listitem table.
        /// </summary>
        public int? AstCommentTwoId { get; set; }

        /// <summary>
        /// Gets or sets additional notes for the culture.
        /// </summary>
        public string? AdditionalNotes { get; set; }

        /// <summary>
        /// Gets or sets additional notes for the AST.
        /// </summary>
        public string? AstAdditionalNotes { get; set; }

        /// <summary>
        /// Gets or sets the aloquat ID for the culture.
        /// </summary>
        public string? AloquatId { get; set; }

        /// <summary>
        /// Gets or sets whether the culture should be displayed on the report.
        /// </summary>
        public string? DisplayOnReport { get; set; }

        /// <summary>
        /// Gets or sets additional data in JSON format.
        /// </summary>
        [Jsonb]
        public string? MoreData { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the alerttype table to the culture table.
        /// </summary>
        public int? AlertTypeId { get; set; }

        /// <summary>
        /// Gets or sets the culture number.
        /// </summary>
        public int? CultureNumber { get; set; }

        /// <summary>
        /// Gets or sets self-referencing foreign key linking to the parent culture within the culture table.
        /// </summary>
        public int ParentCultureId { get; set; }
    }
}
