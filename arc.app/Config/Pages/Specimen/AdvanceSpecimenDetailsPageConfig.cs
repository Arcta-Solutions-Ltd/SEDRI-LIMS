using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Provides the configuration for the Point Of Care Details page in create-specimen workflows.
    /// </summary>
    internal class AdvanceSpecimenDetailsPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON definition for the advance specimen details (Point Of Care) page.
        /// </summary>
        /// <returns>A JSON string defining page layout, required fields, and configure actions.</returns>
        public string Get()
        {
            var page = @"{ 
                            name: 'advancespecimendetailspage',
                            pageTitle: '@PoiCar@',
                            text: '@SpeProI@.',
                            configureActions: 'add,edit,delete',
                            required: 'OrganisationId,LaboratoryId',
                            requiredRule: 'and',
                            tablename: 'specimen',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PatientRef', type: 'text', label: '@PatPatB@', Configurable: 'No' },
                                                { id: 'OrganisationId', type: 'hierarchicalpicker', label: '@GenLocA@', required: true, placeholder: '@GenSelJ@', optionsName: 'OrganisationList', dynamic: true, tab: true, Configurable: 'No', allowAdd: true },
                                                { id: 'LaboratoryId', type: 'combobox', label: '@GenLabA@', required: true, placeholder: '@UseSel@', optionsName: 'LaboratoryList', dynamic: true, tab: true, Configurable: 'No' },
                                                { id: 'AdmissionDate', type: 'date', label: '@PatAdm@', required: false, placeholder: '@PatSelF@', Min: 'now y-10', Max: 'now' },
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


