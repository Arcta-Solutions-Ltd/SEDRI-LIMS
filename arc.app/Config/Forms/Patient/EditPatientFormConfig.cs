using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form definition for editing a patient. Marked configurable so patient fields can be added and
    /// edited from Configuration, and so patient fields captured on specimen view create forms can be
    /// referenced onto this form.
    /// </summary>
    internal class EditPatientFormConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON configuration string for the edit patient form.
        /// </summary>
        /// <returns>A JSON string defining the edit patient form.</returns>
        public string Get()
        {
            var form = @"{
                        name: 'editpatientform',
                        title: '@PatEdi@',
                        viewTitle: 'Add a new patient.',
                        initialQuery: 'PatientById',
                        saveEvent: 'editpatient',
                        recordView: 'patientrecordview',
                        configurable: 'Yes',
                        singleItemName: 'patient',
                        configureActions: ['edit'],
                        pages: ['editpatientdetailspage', 'patientaddresspage']
                    }";

            return form;
        }
    }
}
