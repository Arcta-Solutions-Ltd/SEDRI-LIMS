using System.Collections.Generic;

namespace arc.common.Models.Specimen
{
    /// <summary>
    /// Model for setting culture file attachments. Used by SetCultureFileAttachmentsCommand.
    /// </summary>
    public class SetCultureFileAttachmentsModel
    {
        public int CultureId { get; set; }
        public IEnumerable<int> FileAttachmentIds { get; set; }
    }
}
