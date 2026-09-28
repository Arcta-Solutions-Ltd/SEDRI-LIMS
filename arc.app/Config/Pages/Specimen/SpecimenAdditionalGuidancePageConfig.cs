using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the JSON configuration for the specimen additional guidance page on add/edit culture forms.
/// Define Page Contents allows add/edit/delete at page level; all existing fields remain configurable.
/// </summary>
internal class SpecimenAdditionalGuidancePageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the specimen additional guidance page.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing page name, title, text, and column/form-group/field definitions.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'specimenadditionalguidance',
                            pageTitle: '@SpeAddF@',
                            text: '@SpeProC@.',
                            configureActions: 'add,edit,delete',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            rules:[{ effect: 'visible', field: 'OrganismId', rule: 'isnotempty'}],
                                            fields: [
                                                { id: 'IdPercentage', type: 'singleline', label: '@CulIdeCer@', required: false, placeholder: '@CulIdeCerB@', Max: 4 }
                                            ]
                                        },
                                        {
                                            key: 'fg2',
                                            fields: [
                                                { id: 'Comment1', type: 'combobox', label: '@GenCom@', required: false, placeholder: '@GenSelE@', optionsName: 'CannedComments', ParentValue: '1232', IsComment: true  },
                                                { id: 'Comment2', type: 'combobox', label: '@GenComA@', required: false, placeholder: '@GenSelE@', optionsName: 'CannedComments', ParentValue: '1232', IsComment: true  },
                                                { id: 'AdditionalNotes', type: 'multiline', label: '@GenAdd@', required: false, placeholder: '@SpeEntD@', Max: 5000, IsComment: true  },
                                                { id: 'DisplayInReport', type: 'toggle', label: '@GenDisA@', required: false, defaultValue: 'Yes' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
