using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Defines the UI form configuration for batch rejecting reports.
/// </summary>
internal class BatchRejectReportFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch reject report form.
    /// </summary>
    public string Get()
    {
        var form = @"{
            name: 'batchrejectreportform',
            viewTitle: 'Batch reject report',
            saveEvent: 'batchrejectreportevent',
            suppressRecordView: true,
            pages: ['batchrejectreportpage']
        }";

        return form;
    }
}
