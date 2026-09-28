namespace arc.app.Common
{
    /// <summary>
    /// Implemented by an <see cref="IRun"/> event that may create records above the one it returns the id
    /// of, so those ids can be sent back to the client. A form that offers to run again after saving needs
    /// them to attach the next record to the same parents rather than creating duplicates.
    /// </summary>
    public interface ICreateParentRecords
    {
        /// <summary>Patient the created record was attached to, zero when there is none.</summary>
        int CreatedPatientId { get; }

        /// <summary>Admission the created record was attached to, null when there is none.</summary>
        int? CreatedAdmissionId { get; }

        /// <summary>Request the created record was attached to, null when there is none.</summary>
        int? CreatedRequestId { get; }
    }
}
