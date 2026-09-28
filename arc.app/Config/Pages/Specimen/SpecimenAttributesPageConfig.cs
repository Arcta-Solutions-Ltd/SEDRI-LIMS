using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Provides the configuration for the Clinical Information page in create-specimen workflows.
    /// </summary>
    internal class SpecimenAttributesPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON definition for the specimen clinical information page.
        /// </summary>
        /// <returns>A JSON string defining page layout, required fields, and configure actions.</returns>
        public string Get()
        {
            var page = @"{ 
                            name: 'specimenattributes',
                            pageTitle: '@CusCli@',
                            text: '@CusEnt@.',
                            required: 'DiagnosisId',
                            requiredRule: 'or',
                            configureActions: 'add,edit,delete',
                            tablename: 'specimen',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AntibioticsInLast24hrsId', type: 'combobox', label: '@CusAnt@', required: false, placeholder: '@GenSelK@', optionsName: 'AntibioticsInLast24hrs' },
                                                { id: 'TempInLast24hrsId', type: 'combobox', label: '@CusTem@', required: false, placeholder: '@GenSelK@', optionsName: 'TempInLast24hrs' },
                                                { id: 'DiagnosisId', type: 'combobox', label: '@CusSus@', required: true, placeholder: '@GenSelD@', optionsName: 'PatientDiagnosis', defaultValue: '875' },
                                                { id: 'AdditionalClinicalInformation', type: 'multiline', label: '@CusAdd@', required: false, placeholder: '@CusEntA@'}
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
