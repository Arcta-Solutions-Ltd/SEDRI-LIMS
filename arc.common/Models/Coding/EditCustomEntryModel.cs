namespace arc.common.Models.Coding
{
    public class EditCustomEntryModel
    {
        public int Id { get; set; }
        public int AdditionalId { get; set; }
        public int OrganismCodingId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int OrderId { get; set; }
        public int FamilyId { get; set; }
        public int GenusId { get; set; }
        public int SpeciesId { get; set; }
    }
}
