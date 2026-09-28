using System.Collections.Generic;

namespace arc.common.Models.Instruments
{
    /// <summary>
    /// Payload for <c>POST api/instrument/confirm</c> when the interface acknowledges outbound processing for a pending instrument result.
    /// </summary>
    public class RequestConfirmModel
    {
        /// <summary>
        /// The <c>instrumentresults.id</c> row to move to status &quot;Requested&quot; (883).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Instrument profile name (informational for callers).
        /// </summary>
        public string InstrumentName { get; set; }

        /// <summary>
        /// Optional <c>fileattachments.id</c> values from <c>POST api/file/upload</c> (typically category <c>instrument</c>) to link to this result as outbound/source traceability files.
        /// Same junction table as inbound <see cref="ResponseModel.SourceFileAttachmentIds"/>.
        /// </summary>
        public List<int> SourceFileAttachmentIds { get; set; } = new List<int>();
    }
}
