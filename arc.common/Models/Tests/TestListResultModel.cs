using System;

namespace arc.common.Models.Tests
{
    public class TestListResultModel
    {
        public string Id { get; set; }
        public string TestName { get; set; }
        public string TestDescription { get; set; }
        public string Status { get; set; }
        public DateTime Requested { get; set; }
        public DateTime? Completed { get; set; }
        public string TestResults { get; set; }
        public string Colour { get; set; }
        public int AlertCategoryId { get; set; }
        public string AccessionNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PatientRef { get; set; }
        public string SpecimenType { get; set; }
        public string CultureType { get; set; }
        public int? SpecimenId { get; set; }
        public int? LaboratoryId { get; set; }
        public string TurnAroundTimeColour { get; set; }
    }
}
