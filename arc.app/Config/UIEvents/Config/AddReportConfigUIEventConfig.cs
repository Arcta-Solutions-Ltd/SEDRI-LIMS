using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the UI event that opens the form to add a new report configuration.
/// This event is triggered from the report list view and opens the addreportconfigform
/// which allows users to clone an existing report configuration.
/// </summary>
internal class AddReportConfigUIEventConfig : IDefinition
{
    /// <summary>
    /// Gets the UI event configuration definition for adding a new report configuration.
    /// </summary>
    /// <returns>A JSON string containing the UI event configuration with name 'addreportconfiguievent',
    /// type 'form', and action 'addreportconfigform'.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addreportconfiguievent',
                        description: 'Add report config',
                        type: 'form',
                        action: 'addreportconfigform'
                    }";

        return newEvent;
    }
}
