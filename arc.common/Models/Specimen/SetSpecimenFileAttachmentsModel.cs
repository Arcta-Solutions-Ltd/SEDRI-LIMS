using System.Collections.Generic;

namespace arc.common.Models.Specimen
{
    /// <summary>
    /// Model for setting specimen file attachments. Used by SetSpecimenFileAttachmentsCommand.
    /// </summary>
    public class SetSpecimenFileAttachmentsModel
    {
        public int SpecimenId { get; set; }
        public IEnumerable<int> FileAttachmentIds { get; set; }
    }
}
