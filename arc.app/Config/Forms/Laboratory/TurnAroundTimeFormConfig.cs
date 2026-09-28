using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Defines the configuration for the "Turn Around Time" form.
/// </summary>
internal class TurnAroundTimeFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the form configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the form structure and associated parameters.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'turnaroundtimeform',
                        title: '@GenTAT@',
                        initialQuery: 'turnaroundtimeforminitialquery',
                        saveEvent: 'updateturnaroundtimeconfig',
                        suppressRecordView: true,
                        pages: ['turnaroundtimepage']
                    }";

        return form;
    }
}
