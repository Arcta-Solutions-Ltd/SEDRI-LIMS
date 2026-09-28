using System;

namespace arc.common.Models.Patient
{
    public class MovePatientModel : PatientModel
    {
        public DateTime? DateOfBirthAsDate { get; set; }
        public string AgeMonths { get; set; }
        public string Gender { get; set; }
        public int PatientId { get; set; }
    }
}
