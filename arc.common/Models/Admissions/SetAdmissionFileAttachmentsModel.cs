using System.Collections.Generic;

namespace arc.common.Models.Admissions;

/// <summary>
/// Model for setting admission file attachments. Used by SetAdmissionFileAttachmentsCommand.
/// </summary>
public class SetAdmissionFileAttachmentsModel
{
    public int AdmissionId { get; set; }
    public IEnumerable<int> FileAttachmentIds { get; set; }
}
