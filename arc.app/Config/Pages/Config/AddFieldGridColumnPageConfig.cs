using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Page for adding a fieldgrid column. Used as subform with deferSave.
    /// List and MultiSelect are visible only for combobox and dropdown column types.
    /// Field options for RulesEditor passed via localFormData.fieldOptions from parent.
    /// </summary>
    internal class AddFieldGridColumnPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON page configuration for the add grid column subform.
        /// </summary>
        public string Get()
        {
            var page = @"{
                            name: 'addfieldgridcolumnpage',
                            pageTitle: '@ConAddGC@',
                            text: '',
                            required: 'GridId,GridType,GridWidth',
                            requiredRule: 'and',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GridId', type: 'singleline', label: '@GenNam@', required: true },
                                                { id: 'GridTitle', type: 'singleline', label: '@ConGriHP@' },
                                                { id: 'GridType', type: 'dropdown', label: '@GenTyp@', optionsName: 'gridfieldtypelist', width: 'medium' },
                                                { id: 'GridWidth', type: 'dropdown', label: '@GenWid@', optionsName: 'gridwidth', width: 'small', required: true }
                                            ]
                                        },
                                        {
                                            key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'GridType', rule: '=', value: '453, 454'}],
                                            fields: [
                                                { id: 'GridOption', type: 'dropdown', label: '@GenLisB@', optionsName: 'commonlist', width: 'medium' },
                                                { id: 'MultiSelect', type: 'toggle', label: '@ConMul@' }
                                            ]
                                        },
                                        {
                                            key: 'fg3',
                                            fields: [
                                                { id: 'Rules', type: 'ruleseditor', label: '@GenRul@', context: 'gridcolumn' }
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
