using System;

namespace arc.common.Models.Specimen
{
    public class SpecimenApprovalModel
    {
        public string SubmittedBy { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
