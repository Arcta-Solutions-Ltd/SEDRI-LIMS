using System;

namespace arc.domain.Instruments
{
    public class InstrumentResult
    {
        public int Id { get; set; }
        public string InstrumentName { get; set; }
        public int SpecimenId { get; set; }
        public int CultureId { get; set; }
        public string Barcode { get; set; }
        public bool Enabled { get; set; }
        public DateTime RequestMade { get; set; }
        public DateTime ResultReceived { get; set; }
        public int StatusId { get; set; }
        /// <summary>
        /// Optional listitem id on the InstrumentMachine list (same as <see cref="SingleInstrumentConfig.InstrumentMachineId"/>).
        /// </summary>
        public int? InstrumentMachineId { get; set; }
        /// <summary>
        /// Copy of specimen accession at insert time (inbound matching).
        /// </summary>
        public string AccessionNumber { get; set; }
        /// <summary>
        /// Copy of culture number when <see cref="CultureId"/> is set (inbound matching).
        /// </summary>
        public string CultureNumber { get; set; }
        public string MoreData { get; set; }
        public string RawResult { get; set; }
    }
}
