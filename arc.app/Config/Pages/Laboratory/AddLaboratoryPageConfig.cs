using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Add Laboratory" page.
/// </summary>
internal class AddLaboratoryPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Laboratory" page.
    /// </summary>
    /// <remarks>
    /// This configuration defines the page's structure, including its name, title, text, columns, 
    /// and form groups with fields for entering laboratory details.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, containing metadata, 
    /// field definitions, and layout details.
    /// </returns>
    public string Get()
    {
        var page = @"{
                        name: 'addlaboratorypage',
                        pageTitle: '@LabAdd@',
                        text: '@LabEntA@.',
                        columns: [
                            {
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'LaboratoryName', type: 'singleline', label: '@LabLabA@', required: true, placeholder: '@LabEnt@', Max: 80 },
                                            { id: 'LanguageId', type: 'dropdown', label: '@GenLan@', required: true, placeholder: '@LanSel@', optionsName: 'Translation', dynamic: true },
                                            { id: 'CodingListId', type: 'dropdown', label: '@GenOrgF@', placeholder: '@GenOrgF@', optionsName: 'Coding', dynamic: true, required: false, multiSelect: true, removeFixed: true },
                                            { id: 'AntibioticGroupIds', type: 'dropdown', label: '@AntAnt@', placeholder: '@AntAnt@', optionsName: 'antibioticgroup', dynamic: true, multiSelect: true },
                                            { id: 'ResistanceMechanismIsolateTestNames', type: 'combobox', label: '@LabRmA@', placeholder: '@LabRmA@', optionsName: 'culturetestconfiglist', tab: true, multiSelect: true, dynamic: true, translateOptions: true },
                                            { id: 'DefaultWorkflowId', type: 'combobox', label: '@GenDefA@', required: true, placeholder: '@ConSelF@', optionsName: 'WorkflowList', tab: true, dynamic: true },
                                            { id: 'RecordSusceptibilityChangeAudit', type: 'toggle', label: '@LabSusAud@', value: 'No' }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
