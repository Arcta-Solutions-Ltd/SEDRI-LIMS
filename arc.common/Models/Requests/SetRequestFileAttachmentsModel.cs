using System.Collections.Generic;

namespace arc.common.Models.Requests;

/// <summary>
/// Model for setting request file attachments. Used by SetRequestFileAttachmentsCommand.
/// </summary>
public class SetRequestFileAttachmentsModel
{
    public int RequestId { get; set; }
    public IEnumerable<int> FileAttachmentIds { get; set; }
}
