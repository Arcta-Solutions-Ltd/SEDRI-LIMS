using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page configuration for the patient identification step of the Neoshield neonatal request
/// (specification fields 1 and 3 to 8). Only shown when the user is registering a new baby.
/// </summary>
internal class NeoshieldPatientIdentificationPageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the Neoshield patient identification page.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'neoshieldpatientidentificationpage',
                        pageTitle: '@NeoPatIde@',
                        text: '@NeoPatIdeA@.',
                        required: 'PatientRef,MotherRecordAvailable,BabyNamed,Gender',
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
                                            { id: 'PatientRef', type: 'singleline', label: '@PatPatB@', required: true, placeholder: '@PatEntF@', Max: 20 },
                                            { id: 'MotherRecordAvailable', type: 'radio', label: '@NeoMotAva@', required: true, optionsName: 'NeoYesNo' }
                                        ]
                                    },
                                    {
                                        key: 'fg2',
                                        rules: [{ effect: 'visible', field: 'MotherRecordAvailable', rule: '=', value: '1940' }],
                                        fields: [
                                            { id: 'MotherPatientRef', type: 'singleline', label: '@NeoMotRef@', placeholder: '@PatEntF@', Max: 20 }
                                        ]
                                    },
                                    {
                                        key: 'fg3',
                                        fields: [
                                            { id: 'BabyNamed', type: 'radio', label: '@NeoBabNam@', required: true, optionsName: 'NeoBabyNamed' }
                                        ]
                                    },
                                    {
                                        key: 'fg4',
                                        rules: [{ effect: 'visible', field: 'BabyNamed', rule: '=', value: '1945' }],
                                        fields: [
                                            { id: 'FirstName', type: 'singleline', label: '@NeoBabFir@', placeholder: '@PatEntA@', Max: 50 },
                                            { id: 'Surname', type: 'singleline', label: '@NeoBabLas@', placeholder: '@PatEntB@', Max: 50 }
                                        ]
                                    },
                                    {
                                        key: 'fg5',
                                        fields: [
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
