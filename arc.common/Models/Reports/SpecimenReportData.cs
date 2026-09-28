namespace arc.common.Models.Reports
{
    public class SpecimenReportData
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientLocation { get; set; }
        public string AdmissionDate { get; set; }
        public string Diagnosis { get; set; }
        public string AccessionNumber { get; set; }
        public string Barcode { get; set; }
        public string SpecimenType { get; set; }
        public string SpecimenSite {get; set;}
        public string BottleOnlyWeight { get; set; }
        public string BloodAndBottleWeight { get; set; }
        public string ReceivedCondition { get; set; }
        public string SpecimenAppearance { get; set; }
        public string SpecimenWeight { get; set; }
        public string CollectionDate { get; set; }
        public string CollectionTime { get; set; }
        public string ReceivedDate { get; set; }
        public string ReceivedTime { get; set; }
        public string State { get; set; }
        public string Organisation { get; set; }
        public string Growth { get; set; }
        public string ReasonOne { get; set; }
        public string ReasonTwo { get; set; }
        public string Laboratory { get; set; }

    }
}
