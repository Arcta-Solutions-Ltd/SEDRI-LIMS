using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Add Patient Tag" form.
/// Allows adding a tag to a patient from the patient record view.
/// </summary>
internal class AddPatientTagFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Patient Tag" form.
    /// </summary>
    public string Get()
    {
        var form = @"{
                        name: 'addpatienttagform',
                        viewTitle: '@GenTagB@',
                        saveEvent: 'addpatienttag',
                        recordView: 'patientrecordview',
                        suppressRecordView: false,
                        InitialQuery: 'AddPatientTagFormInitialQuery',
                        pages: [ 'addpatienttagpage' ]
                    }";

        return form;
    }
}
