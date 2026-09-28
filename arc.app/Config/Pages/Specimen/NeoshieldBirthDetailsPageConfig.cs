using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page configuration for the birth details step of the Neoshield neonatal request
/// (specification fields 9 to 14). Only shown when the user is registering a new baby.
/// </summary>
internal class NeoshieldBirthDetailsPageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the Neoshield birth details page.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'neoshieldbirthdetailspage',
                        pageTitle: '@NeoBirAdm@',
                        text: '@NeoBirAdmA@.',
                        required: 'BirthDateTimeKnown,BirthWeightAvailable,InbornOutborn',
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
                                            { id: 'BirthDateTimeKnown', type: 'combobox', label: '@NeoBirKno@', required: true, multiselect: true, placeholder: '@GenSelK@', optionsName: 'NeoDateTimeKnown' }
                                        ]
                                    },
                                    {
                                        key: 'fg2',
                                        rules: [{ effect: 'visible', field: 'BirthDateTimeKnown', rule: 'contains', value: '1947' }],
                                        fields: [
                                            { id: 'DateOfBirth', type: 'date', label: '@PatDat@', placeholder: '@PatSel@', Min: 'now y-2', Max: 'now' },
                                            { id: 'PatientAgeDisplay', type: 'singleline', label: '@PatAge@', ReadOnly: true, Configurable: 'No' }
                                        ]
                                    },
                                    {
                                        key: 'fg3',
                                        rules: [{ effect: 'visible', field: 'BirthDateTimeKnown', rule: 'contains', value: '1948' }],
                                        fields: [
                                            { id: 'TimeOfBirth', type: 'time', label: '@NeoBirTim@', placeholder: '@SpeEntI@', mask: '99:99' }
                                        ]
                                    },
                                    {
                                        key: 'fg4',
                                        fields: [
                                            { id: 'BirthWeightAvailable', type: 'radio', label: '@NeoBirWeiAva@', required: true, optionsName: 'NeoYesNoUnknown' }
                                        ]
                                    },
                                    {
                                        key: 'fg5',
                                        rules: [{ effect: 'visible', field: 'BirthWeightAvailable', rule: '=', value: '1942' }],
                                        fields: [
                                            { id: 'BirthWeight', type: 'number', label: '@NeoBirWei@', min: 300, max: 6000 }
                                        ]
                                    },
                                    {
                                        key: 'fg6',
                                        fields: [
                                            { id: 'InbornOutborn', type: 'radio', label: '@NeoInbOut@', required: true, optionsName: 'NeoInbornOutborn' }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
