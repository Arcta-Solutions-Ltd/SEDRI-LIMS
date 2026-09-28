using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Provides the configuration for the Unapprove Report form.
/// </summary>
internal class UnapproveReportFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the Unapprove Report form configuration.
    /// </summary>
    /// <returns>A JSON string defining the form configuration.</returns>
    public string Get()
    {
        var form = @"{
            name: 'unapprovereportform',
            viewTitle: 'Unapprove Report',
            saveEvent: 'unapprovereportevent',
            RecordView: 'specimenrecordview',
            DisplaySettings: 'fullScreenDefault',
            InitialQuery: 'ApproveReportFormQuery',
            suppressRecordView: false,
            pages: ['unapprovereportpage']
        }";

        return form;
    }
}
