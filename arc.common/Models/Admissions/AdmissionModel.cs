using System;

namespace arc.common.Models.Admissions
{
    /// <summary>
    /// Represents a row in the Admission table. An admission groups the requests and specimens raised for a
    /// patient during a single stay. Every field beyond the columns below is held in <see cref="MoreData"/>.
    /// </summary>
    public class AdmissionModel
    {
        /// <summary>Primary key of the admission.</summary>
        public int Id { get; set; }

        /// <summary>Patient the admission belongs to.</summary>
        public int PatientId { get; set; }

        /// <summary>Json blob holding the admission fields that have no dedicated column.</summary>
        public string MoreData { get; set; }

        /// <summary>Timestamp of the last change to the admission.</summary>
        public DateTime LastModifiedDate { get; set; }
    }
}
