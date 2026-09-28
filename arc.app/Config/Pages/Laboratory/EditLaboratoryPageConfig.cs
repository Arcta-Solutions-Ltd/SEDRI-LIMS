using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Defines the configuration for the "Edit Laboratory" page.
/// </summary>
internal class EditLaboratoryPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the page structure and form elements.</returns>
    public string Get()
    {
        var page = @"{ 
                        name: 'editlaboratorypage',
                        pageTitle: '@LabEdiA@',
                        text: '@LabEdi@.',
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
                                            { id: 'CodingListId', type: 'dropdown', label: '@GenOrgF@', placeholder: '@GenOrgF@', optionsName: 'Coding', dynamic: true, required: false, multiSelect: true, includeFixed: false, removeFixed: true },
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
