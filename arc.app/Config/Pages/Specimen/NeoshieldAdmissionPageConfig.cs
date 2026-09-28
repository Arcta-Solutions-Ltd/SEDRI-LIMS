using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page configuration for the admission step of the Neoshield neonatal request
/// (specification fields 15 to 17). Only shown when the user chose to create a new admission.
/// </summary>
internal class NeoshieldAdmissionPageConfig : IDefinition
{
    /// <summary>
    /// Constructs and returns the JSON configuration string for the Neoshield admission page.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
                        name: 'neoshieldadmissionpage',
                        pageTitle: '@NeoAdm@',
                        text: '@NeoAdmA@.',
                        required: 'AdmissionDateTimeKnownId',
                        requiredRule: 'and',
                        pageGroup: 'admission',
                        tablename: 'admission',
                        columns: [
                            {
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'AdmissionDateTimeKnownId', type: 'combobox', label: '@NeoAdmKno@', required: true, multiselect: true, placeholder: '@GenSelK@', optionsName: 'NeoDateTimeKnown' }
                                        ]
                                    },
                                    {
                                        key: 'fg2',
                                        rules: [{ effect: 'visible', field: 'AdmissionDateTimeKnownId', rule: 'contains', value: '1947' }],
                                        fields: [
                                            { id: 'DateOfAdmission', type: 'date', label: '@NeoAdmDat@', placeholder: '@PatSelF@', Min: 'now y-2', Max: 'now' }
                                        ]
                                    },
                                    {
                                        key: 'fg3',
                                        rules: [{ effect: 'visible', field: 'AdmissionDateTimeKnownId', rule: 'contains', value: '1948' }],
                                        fields: [
                                            { id: 'TimeOfAdmission', type: 'time', label: '@NeoAdmTim@', placeholder: '@SpeEntI@', mask: '99:99' }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
