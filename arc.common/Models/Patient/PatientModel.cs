namespace arc.common.Models.Patient
{
    /// <summary>
    /// Model representing the structure of the Patient table.
    /// </summary>
    public class PatientModel
    {
        public int Id { get; set; }
        public string PatientRef { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string DateOfBirth { get; set; }
        public string Age { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int LocationId { get; set; }
        public string ZipCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string MoreData { get; set; }
        public int GenderId { get; set; }
        public string Barcode { get; set; }
    }
}
