namespace arc.common.Models.Files;

/// <summary>
/// Request model for ManageSpecimenAttachments and ManageCultureAttachments events.
/// Id is the record id (specimen or culture). FileAttachmentIds is comma-separated file attachment ids.
/// </summary>
public class ManageAttachmentsRequestModel
{
    /// <summary>Record id (specimen or culture). From record view context when form is opened.</summary>
    public int Id { get; set; }

    /// <summary>Comma-separated file attachment ids. Empty or null means no attachments.</summary>
    public string FileAttachmentIds { get; set; }
}
