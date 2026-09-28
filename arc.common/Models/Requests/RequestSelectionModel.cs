using System;

namespace arc.common.Models.Requests
{
    /// <summary>
    /// A single card on the request selection screen. The descriptive values live in the request MoreData blob
    /// and are resolved against ListItem here so the card can be read without a further lookup.
    /// </summary>
    public class RequestSelectionModel
    {
        /// <summary>Primary key of the request.</summary>
        public int Id { get; set; }

        /// <summary>Patient the request belongs to.</summary>
        public int PatientId { get; set; }

        /// <summary>Admission the request belongs to, or null when the request has no admission.</summary>
        public int? AdmissionId { get; set; }

        /// <summary>Human readable request reference.</summary>
        public string RequestId { get; set; }

        /// <summary>Date the request was submitted, in dd/MM/yyyy form.</summary>
        public string RequestDate { get; set; }

        /// <summary>Time the request was submitted, in 24 hour hh:mm form.</summary>
        public string RequestTime { get; set; }

        /// <summary>Resolved ward name for the request.</summary>
        public string Ward { get; set; }

        /// <summary>Resolved urgency for the request.</summary>
        public string Urgency { get; set; }

        /// <summary>Resolved indication for the request.</summary>
        public string Indication { get; set; }

        /// <summary>Number of specimens already taken against this request.</summary>
        public int SpecimenCount { get; set; }

        /// <summary>Timestamp of the last change to the request, used to order the cards.</summary>
        public DateTime LastModifiedDate { get; set; }
    }
}
