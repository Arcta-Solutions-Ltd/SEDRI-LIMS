namespace arc.common.Models.Specimen
{
    public class SpecimenBatchListModel
    {
        public int Id { get; set; }
        public string SpecimenType { get; set; }
        public int OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public string Printed { get; set; }
        public string Published { get; set; }
        public string AccessionNumber { get; set; }
        public string Surname { get; set; }
        public string DateFinalised { get; set; }
    }
}
