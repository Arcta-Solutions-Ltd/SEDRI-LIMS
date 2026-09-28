using System;

namespace arc.domain.Tests
{
    public class Test
    {
        public int Id { get; set; }
        public int SpecimenId { get; set; }
        public int CultureId { get; set; }
        public int StateId { get; set; }
        public string TestName { get; set; }
        public string TestDescription { get; set; }
        public string TestResults { get; set; }
        public string Status { get; set; }
        public string Requested { get; set; }
        public string Completed { get; set; }
        public int AlertCategoryId { get; set; }
        public string Colour { get; set; }
        public string TurnAroundTimeColour { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
