using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the JSON configuration for the Edit Workflow Entry page.
/// This configuration defines the layout for editing a workflow entry including:
/// - The page's name, title, and descriptive text.
/// - Styling properties such as "crafted" and "wide".
/// - Required fields along with the corresponding validation rule.
/// - A layout with columns that contain form groups and fields.
/// 
/// In detail, the configuration includes:
/// 1. A column (col1) that spans a wide area, with form groups arranged as follows:
///    - Form Group "fg1" includes:
///         • EventField: A required text field with a placeholder for event selection.
///         • EntryStates: A required combobox field with dynamic options to choose workflow entry states.
///         • DefaultExitStates: An optional combobox field with dynamic options for default exit states.
///    - Form Group "fg2" includes a visibility rule that makes this group visible when the 'Event' field is not empty,
///      and contains a crafted field "WorkflowRuleGrid". This grid displays several dropdown and input fields
///      designed for configuring workflow rules, such as ExitState, Field, and value fields (StringValue, NumberValue, ListValue).
/// </summary>
/// <returns>
/// A JSON string representing the page configuration for editing workflow entries.
/// </returns>
internal class EditWorkflowEntryPageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{ 
                            name: 'editworkflowentrypage',
                            pageTitle: '@ConEdiT@',
                            text: '@ConEdiU@.',
                            crafted: true,
                            wide: true,
                            required: 'EntryStates',
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
                                                { id: 'EventField', type: 'text', label: '@GenEve@', required: true, placeholder: '@GenSelO@' },
                                                { id: 'EntryStates', type: 'combobox', label: '@ConEnt@', required: true, placeholder: '@ConSelE@', optionsName: 'specimenworkflowitems', dynamic: true },
                                                { id: 'DefaultExitStates', type: 'combobox', label: '@ConDefC@', required: false, placeholder: '@ConSelE@', optionsName: 'specimenworkflowitems', dynamic: true }
                                            ]
                                        },
                                        { 
                                            key: 'fg2',
                                            rules: [{ effect: 'visible', field: 'Event', rule: 'isnotempty'}],
                                            fields: [
                                                { id: 'WorkflowRuleGrid', type: 'crafted', label: '@ConCon@', gridfields: [
                                                        { id: 'ExitState', type: 'dropdown', optionsName: 'specimenworkflowitems', GridTitle: '@ConExi@' },
                                                        { id: 'Field', type: 'dropdown', optionsName: 'fieldlist', dynamic: true, GridTitle: '@GenFieA@' },
                                                        { id: 'StringValue', type: 'singleline', width: 'medium' },
                                                        { id: 'NumberValue', type: 'number', width: 'medium' },
                                                        { id: 'ListValue', type: 'dropdown', width: 'medium' }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
