using System.Collections.Generic;

namespace arc.common.Models.Patient
{
    /// <summary>
    /// Model for setting patient file attachments. Used by SetPatientFileAttachmentsCommand.
    /// </summary>
    public class SetPatientFileAttachmentsModel
    {
        public int PatientId { get; set; }
        public IEnumerable<int> FileAttachmentIds { get; set; }
    }
}
