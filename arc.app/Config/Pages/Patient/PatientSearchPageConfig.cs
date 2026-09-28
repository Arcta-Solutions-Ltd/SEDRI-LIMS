using arc.app.Common;

namespace arc.app.Config.Pages.Patient
{
    /// <summary>
    /// Represents the configuration for the workflow patient search page.
    /// Implements the IDefinition interface to provide a JSON-based configuration string.
    /// </summary>
    internal class PatientSearchPageConfig : IDefinition
    {
        /// <summary>
        /// Constructs and returns the JSON configuration string for the patient search page.
        /// </summary>
        /// <returns>A JSON string defining the patient search page configuration.</returns>
        public string Get()
        {
            var page = @"{ 
                            name: 'patientsearchpage',
                            pageGroup: 'patient',
                            groupAnchor: 1,
                            pageTitle: '@PatPatF@',
                            text: '@PatSea@.',
                            required: 'Values,SurnameSearch,PatientRefSearch,LocationSearch,AgeFromYears,AgeFromMonths,AgeFromDays,AgeFromHours,AgeToYears,AgeToMonths,AgeToDays,AgeToHours,DateOfBirthSearch',
                            requiredRule: 'or',
                            requiredError: '@ValOr@',
                            tableName: 'None',
                            configureActions: '',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PatientRefSearch', type: 'singleline', label: '@PatPatB@', placeholder: '@PatEntF@', Configurable: 'No' },
                                                { id: 'Values', type: 'singleline', label: '@PatFir@', placeholder: '@PatEntA@', Configurable: 'No' },
                                                { id: 'SurnameSearch', type: 'singleline', label: '@PatSurA@', placeholder: '@PatEntB@', Configurable: 'No' },
                                                { id: 'LocationSearch', type: 'hierarchicalpicker', placeholder: '@LocSel@', label: '@GenLoc@', optionsName: 'LocationList', Configurable: 'No', allowAdd: false },
                                                { id: 'AgeFrom', type: 'age', label: '@PatAgeC@', required: false, Configurable: 'No' },
                                                { id: 'AgeTo', type: 'age', label: '@PatAgeD@', required: false, Configurable: 'No' },
                                                { id: 'DateOfBirthSearch', type: 'date', label: '@PatDat@', placeholder: '@PatSel@', Min: 'now y-130', Max: 'now', Configurable: 'No' }
                                            ],
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
