using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Represents the configuration for the patient details page.
/// Implements the IDefinition interface to provide a JSON-based configuration string.
/// </summary>
internal class PatientDetailsPageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the patient details page.
    /// </summary>
    /// <returns>A JSON string defining the patient details page configuration.</returns>
    public string Get()
    {
        var page = @"{ 
                            name: 'patientdetailspage',
                            pageTitle: '@PatAddA@',
                            text: '@PatEntE@.',
                            required: 'PatientRef,FirstName,Surname,Gender',
                            configureActions: 'add,edit,delete',
                            requiredRule: 'and',
                            pageGroup: 'patient',
                            tablename: 'patient',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PatientRef', type: 'singleline', label: '@PatPatB@', required: true, placeholder: '@PatEntF@', Configurable: 'No', Max: 20 },
                                                { id: 'FirstName', type: 'singleline', label: '@PatFir@', required: true, placeholder: '@PatEntA@', Max: 50 },
                                                { id: 'Surname', type: 'singleline', label: '@PatSurA@', required: true, placeholder: '@PatEntB@', Max: 50 },
                                                { id: 'DateOfBirth', type: 'date', label: '@PatDat@', placeholder: '@PatSel@', Min: 'now y-130', Max: 'now', Configurable: 'No' },
                                                { id: 'PatientAgeDisplay', type: 'singleline', label: '@PatAge@', ReadOnly: true, Configurable: 'No' },
                                                { id: 'Gender', type: 'combobox', label: '@PatGenA@', required: true, optionsName: 'gender', placeholder: '@PatSelA@' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}


