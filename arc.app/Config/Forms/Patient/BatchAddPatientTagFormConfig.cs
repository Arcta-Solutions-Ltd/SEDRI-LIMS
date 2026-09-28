using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the batch add patient tag form.
/// Allows adding tags to multiple patients in batch from the patient list view.
/// </summary>
internal class BatchAddPatientTagFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the form configuration.
    /// </summary>
    public string Get()
    {
        var form = @"{
                        name: 'batchaddpatienttagform',
                        viewTitle: '@GenTagB@',
                        saveEvent: 'batchaddpatienttag',
                        recordView: 'patientrecordview',
                        suppressRecordView: true,
                        pages: ['batchaddpatienttagpage']
                    }";

        return form;
    }
}
