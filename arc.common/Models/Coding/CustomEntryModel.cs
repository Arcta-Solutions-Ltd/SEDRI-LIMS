namespace arc.common.Models.Coding
{
    public class CustomEntryModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string MetafCodingId {get; set;}
        public string Code { get; set; }
        public int OrderId { get; set; }
        public int FamilyId { get; set; }
        public int GenusId { get; set; }
        public int SpeciesId { get; set; }
    }
}
