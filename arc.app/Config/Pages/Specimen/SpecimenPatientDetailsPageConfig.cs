using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SpecimenPatientDetailsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'specimenpatientdetails',
                            pageTitle: '@PoiCar@',
                            text: '@SpeProI@.',
                            required: 'OrganisationId',
                            requiredRule: 'and',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PatientRef', type: 'text', label: '@PatPatB@' },
                                                { id: 'OrganisationId', type: 'combobox', label: '@GenLocA@', required: true, placeholder: '@GenSelC@', optionsName: 'OrganisationList', dynamic: true, tab: true },
                                                { id: 'AdmissionDate', type: 'date', label: '@PatAdm@', required: false, placeholder: '@PatSelF@', Min: 'now y-10', Max: 'now'},
                                                { id: 'ClinicalContactNo', type: 'singleline', label: '@PatCli@', required: false, placeholder: '@PatEntG@', Max: 20 }
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


//{ id: 'OrganisationId', type: 'dropdown', label: '@GenWar@', required: true, placeholder: '@GenSelC@', optionsName: 'OrganisationList', tab: true },

//{ id: 'PatientLocationId', type: 'combobox', label: '@PatPatJ@', required: false, placeholder: '@GenSelB@', optionsName: 'PatientLocation' },
