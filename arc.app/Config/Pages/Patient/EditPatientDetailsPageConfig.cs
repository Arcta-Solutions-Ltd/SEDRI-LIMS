using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Represents the configuration for the edit patient details page. The <c>patient</c> table name
    /// makes this a patient scoped page, so it may only reference existing patient fields.
    /// </summary>
    internal class EditPatientDetailsPageConfig : IDefinition
    {
        /// <summary>
        /// Constructs and returns the JSON configuration string for the edit patient details page.
        /// </summary>
        /// <returns>A JSON string defining the edit patient details page configuration.</returns>
        public string Get()
        {
            var page = @"{ 
                            name: 'editpatientdetailspage',
                            pageTitle: '@PatPatD@',
                            text: '@PatEdi@.',
                            configureActions: 'add,edit,delete',
                            pageGroup: 'patient',
                            tablename: 'patient',
                            required: 'PatientRef,FirstName,Surname,Gender',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PatientRef', type: 'singleline', label: '@PatPatB@', required: true, Max: 20 },
                                                { id: 'FirstName', type: 'singleline', label: '@PatFir@', required: true, placeholder: '@PatEntA@', Max: 50 },
                                                { id: 'Surname', type: 'singleline', label: '@PatSurA@', required: true, placeholder: '@PatEntB@', Max: 50 },
                                                { id: 'DateOfBirth', type: 'date', label: '@PatDat@', placeholder: '@PatSel@', Min: 'now y-130', Max: 'now' },
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
}


