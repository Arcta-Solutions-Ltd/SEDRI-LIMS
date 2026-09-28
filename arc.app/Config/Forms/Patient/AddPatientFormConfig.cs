using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddPatientFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addpatientform',
                        viewTitle: 'Add a new patient.',
                        saveEvent: 'addpatient',
                        suppressRecordView: true,
                        pages: ['patientdetailspage','patientaddresspage']
                    }";

            return form;
        }
    }
}
