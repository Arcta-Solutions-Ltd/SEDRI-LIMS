using arc.common.Models.Role;
using System.Collections.Generic;

namespace arc.common.Models.Specimen
{
    public class ACKReceiptEventModel
    {
        public int Id { get; set; }
        public string ReceivedDate { get; set; }
        public string ReceivedTime { get; set; }
        public int ReceivedConditionId { get; set; }
        public int SpecimenAppearanceId { get; set; }
        public decimal? BottleOnlyWeight { get; set; }
        public decimal? BloodandBottleWeight { get; set; }
        public string RejectionReason { get; set; }
        public string SelectReasonId { get; set; }
        public string ManufacturersBarcode { get; set; }
        public string StateId { get; set; }
        public string View { get; set; }
        public List<MenuPermissionEventModel> Crafted { get; set; }
    }
}
