using System;

namespace arc.common.Models.Requests
{
    /// <summary>
    /// Represents a row in the Request table. A request is one clinical decision to investigate, and groups
    /// every specimen taken on that decision. Every field beyond the columns below is held in
    /// <see cref="MoreData"/>.
    /// </summary>
    public class RequestModel
    {
        /// <summary>Primary key of the request.</summary>
        public int Id { get; set; }

        /// <summary>Patient the request belongs to.</summary>
        public int PatientId { get; set; }

        /// <summary>Admission the request belongs to, or null when the request has no admission.</summary>
        public int? AdmissionId { get; set; }

        /// <summary>Human readable request reference, generated when the request is created.</summary>
        public string RequestId { get; set; }

        /// <summary>Json blob holding the request fields that have no dedicated column.</summary>
        public string MoreData { get; set; }

        /// <summary>Timestamp of the last change to the request.</summary>
        public DateTime LastModifiedDate { get; set; }
    }
}
