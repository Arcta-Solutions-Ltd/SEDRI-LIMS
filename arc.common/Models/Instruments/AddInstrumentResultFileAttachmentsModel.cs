using System.Collections.Generic;

namespace arc.common.Models.Instruments;

/// <summary>
/// Model for appending file attachment links to an instrument result row (see add command in arc.data Instruments).
/// </summary>
public class AddInstrumentResultFileAttachmentsModel
{
    public int InstrumentResultId { get; set; }
    public IEnumerable<int> FileAttachmentIds { get; set; }
}
