using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Defines the UI form configuration for batch approving reports.
/// </summary>
internal class BatchApproveReportFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch approve report form.
    /// </summary>
    public string Get()
    {
        var form = @"{
            name: 'batchapprovereportform',
            viewTitle: 'Batch approve Report',
            saveEvent: 'batchapprovereportevent',
            suppressRecordView: true,
            pages: ['batchapprovereportpage']
        }";

        return form;
    }
}
