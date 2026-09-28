using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the form used to add a new report configuration by cloning an existing report.
/// This form is opened by the 'addreportconfiguievent' UI event and allows users to select
/// a report to clone and provide a new title. When saved, it triggers the 'addreportconfig' event
/// which creates a new report with cloned sections.
/// </summary>
internal class AddReportConfigFormConfig : IDefinition
{
    /// <summary>
    /// Gets the form configuration definition for adding a new report configuration.
    /// </summary>
    /// <returns>A JSON string containing the form configuration with name 'addreportconfigform',
    /// saveEvent 'addreportconfig', and page 'addreportconfigpage'.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'addreportconfigform',
                        viewTitle: 'Add report config.',
                        saveEvent: 'addreportconfig',
                        suppressRecordView: true,
                        pages: [ 'addreportconfigpage']
                    }";

        return form;
    }
}
