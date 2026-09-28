using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Provides the configuration for the Approve Report form.
/// </summary>
internal class ApproveReportFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the Approve Report form configuration.
    /// </summary>
    /// <returns>A JSON string defining the form configuration.</returns>
    public string Get()
    {
        var form = @"{
            name: 'approvereportform',
            viewTitle: 'Approve Report',
            saveEvent: 'approvereportevent',
            RecordView: 'specimenrecordview',
            DisplaySettings: 'fullScreenDefault',
            InitialQuery: 'ApproveReportFormQuery',
            suppressRecordView: false,
            pages: ['approvereportpage']
        }";

        return form;
    }
}
