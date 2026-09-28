using System;

namespace arc.common.Models.Admissions
{
    /// <summary>
    /// A single card on the admission selection screen. The admission date and time live in the admission
    /// MoreData blob and are surfaced here so the list can be shown in reverse chronological order.
    /// </summary>
    public class AdmissionSelectionModel
    {
        /// <summary>Primary key of the admission.</summary>
        public int Id { get; set; }

        /// <summary>Patient the admission belongs to.</summary>
        public int PatientId { get; set; }

        /// <summary>Date of admission as entered on the request form, in dd/MM/yyyy form.</summary>
        public string AdmissionDate { get; set; }

        /// <summary>Time of admission as entered on the request form, in 24 hour hh:mm form.</summary>
        public string AdmissionTime { get; set; }

        /// <summary>Number of requests already raised against this admission.</summary>
        public int RequestCount { get; set; }

        /// <summary>Number of specimens already taken against this admission.</summary>
        public int SpecimenCount { get; set; }

        /// <summary>Timestamp of the last change to the admission, used to order the cards.</summary>
        public DateTime LastModifiedDate { get; set; }
    }
}
