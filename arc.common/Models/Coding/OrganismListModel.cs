namespace arc.common.Models.Coding
{
    public class OrganismListModel
    {
        public int Id { get; set; }
        public int OrganismCodingId { get; set; }
        public string Description { get; set; }
        public string Gram { get; set; }
        public string Custom { get; set; }
        public string Code { get; set; }
        public string OrderName { get; set; }
        public string FamilyName { get; set; }
        public string PreferredName { get; set; }
        public string Synonyms { get; set; }
    }
}
