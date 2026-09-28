using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class MergePatientFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'mergepatientform',
                        viewTitle: 'Merge one patient with another.',
                        saveEvent: 'mergepatient',
                        initialQuery: 'PatientByIdForMergeQuery',
                        suppressRecordView: true,
                        pages: ['patientsearchpage','patientsearchresultswithnoaddpage','mergepatientpage']
                    }";

            return form;
        }
    }
}
