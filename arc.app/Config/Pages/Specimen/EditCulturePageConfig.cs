using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Page configuration for editing culture metadata (growth, dates, weights, etc.).
/// Includes required fields and conditional visibility rules based on specimen type.
/// </summary>
internal class EditCulturePageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the Edit Culture page.
    /// Defines form groups and fields, including CultureType, growth, and positive date/time.
    /// </summary>
    public string Get()
    {
        var page = @"{
                        name: 'editculturepage',
                        pageTitle: '@SpeGroB@',
                        text: '@SpeProK@.',
                        required: 'CultureType',
                        requiredRule: 'and',
                        columns: [
                            {
                                key: 'col1',
                                formGroups: [
                                    {
                                        key: 'fg1',
                                        fields: [
                                            { id: 'CultureType', type: 'dropdown', label: '@CulTyp@', optionsName: 'CultureType', required: true, defaultValue: '978' }
                                        ]
                                    },
                                    {
                                        key: 'fg2',
                                        rules:[
                                            { effect: 'visible', field: 'specimentypeid', rule: '=', value: '808'}
                                        ],
                                        fields:[
                                            { id: 'CultureBottleWeight', type: 'number', label: '@SpeBot@', required: false },
                                            { id: 'CultureBloodAndBottleWeight', type: 'number', label: '@SpeBlo@', required: false }
                                        ]
                                    },
                                    {
                                        key: 'fg3',
                                        fields: [
                                            { id: 'growthid', type: 'hierarchicalpicker', label: '@SpeGroC@', required: false, placeholder: '@SpeSelH@', optionsName: 'SpecimenGrowth', leavesOnly: true }
                                        ]
                                    },
                                    {
                                        key: 'fg4',
                                        fields: [
                                            { id: 'PositiveDate', type: 'date', label: '@SpePosA@', required: false, placeholder: '@SpeEntG@', Min: 'now d-300', Max: 'now'},
                                            { id: 'PositiveTime', type: 'time', label: '@SpePosB@', required: false, placeholder: '@SpeEntH@', mask: '99:99'}
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}
