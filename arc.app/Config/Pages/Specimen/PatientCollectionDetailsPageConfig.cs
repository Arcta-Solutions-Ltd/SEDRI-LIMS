using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class PatientCollectionDetailsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'patientcollectiondetails',
                            pageTitle: '@PatPatH@',
                            text: '@SpeProB@.',
                            required: 'Diagnosis',
                            requiredRule: 'and',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PatientSearch', type: 'search', label: '@PatPatI@', required: true, placeholder: '@PatFin@'},
                                                { id: 'PatientLocation', type: 'dropdown', label: '@PatPatJ@', required: false, placeholder: '@GenSelB@', optionsName: 'PatientLocation' },
                                                { id: 'PatientWard', type: 'dropdown', label: '@GenWar@', required: false, placeholder: '@GenSelC@', optionsName: 'PatientWard' },
                                                { id: 'AdmissionDate', type: 'date', label: '@PatAdm@', required: false, placeholder: '@PatSelF@', Min: 'now y-10', Max: 'now'},
                                                { id: 'Diagnosis', type: 'combobox', label: '@GenDia@', required: true, placeholder: '@GenSelD@', optionsName: 'PatientDiagnosis' },
                                                { id: 'ClinicalContactNo', type: 'singleline', label: '@PatCli@', required: false, placeholder: '@PatEntG@', Max: 20}
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
