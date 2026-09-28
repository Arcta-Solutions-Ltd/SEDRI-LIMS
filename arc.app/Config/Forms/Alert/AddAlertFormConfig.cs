using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Represents the configuration for the Add Alert form.
/// </summary>
internal class AddAlertFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Add Alert form.
    /// </summary>
    /// <returns>A JSON string representing the Add Alert form configuration.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'addalertform',
                        viewTitle: 'Add a new alert.',
                        saveEvent: 'addalert',
                        suppressRecordView: true,
                        pages: [ 'alertdetailspage', 'alerttestdetailsonlypage']
                    }";

        return form;
    }
}

