using System;

namespace arc.common.Models.Instruments
{
    public class InstrumentSourceModel
    {
        public string PatientRef { get; set; }
        public string AccessionNumber { get; set; }
        public string ManufacturersBarcode { get; set; }
        public DateTime CollectionDate { get; set; }
        public string CollectionTime { get; set; }
    }
}
