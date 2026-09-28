using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration class for the "Patient Comment Form".
/// </summary>
internal class PatientCommentFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Patient Comment Form" as a JSON string.
    /// </summary>
    /// <returns>A JSON string representing the form configuration.</returns>
    public string Get()
    {
        var form = @"{
                            name: 'patientcommentform',
                            formtype: 'singlepage',
                            saveEvent: 'patientcomment',
                            recordView: 'patientrecordview',
                            suppressRecordView: false,
                            pages: [ 'patientcommentpage' ]
                        }";

        return form;
    }
}

