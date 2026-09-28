using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Provides the page configuration definition for editing a field.
    /// This page contains form fields for configuring field properties such as label, type, required status,
    /// validation messages, list options, min/max values, grid fields, and add/delete button visibility.
    /// </summary>
    internal class EditFieldPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON string representation of the edit field page configuration.
        /// </summary>
        /// <returns>A JSON string containing the page configuration with conditional form groups for different field types and their properties.</returns>
        public string Get()
        {
            var page = @"{
                            name: 'editfieldpage',
                            pageTitle: '@ConEdiI@',
                            text: '@ConEdiJ@',
                            wider: false,
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
		                                {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'label', type: 'singleline', label: '@GenLabB@', required: true, placeholder: '@ConEntB@' },
                                                { id: 'TypeId', type: 'combobox', label: '@GenTyp@', required: true, multiselect: false, placeholder: '@ConSelA@', optionsName: 'FieldTypeList' }
                                            ]
                                        },
                                        {
                                            key: 'fg11',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '450, 451, 453, 454, 455, 456, 458'}],
                                            fields: [
                                                { id: 'Placeholder', type: 'singleline', label: '@ConPla@', placeholder: '@ConEntE@' }
                                            ]
                                        },
                                        {
                                            key: 'fg8',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '450 ,451 ,453, 454, 455, 456, 458, 149, 150, 151'}],
                                            fields: [
                                                { id: 'required', type: 'toggle', label: '@GenReqB@', required: true, defaultValue: 'Yes' },
                                                { id: 'ReadOnly', type: 'toggle', label: '@ConRea@', defaultValue: 'No' },
                                            ]
                                        },
                                        {
                                            key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'Required', rule: '=', value: 'Yes'}],
                                            fields: [
                                                { id: 'RequiredErrorMessage', type: 'singleline', label: '@ConReq@', placeholder: '@ConEntD@'  }
                                            ]
                                        },
                                        {
                                            key: 'fg3',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '453, 454, 151'}],
                                            fields: [
                                                { id: 'List', type: 'combobox', label: '@GenLisB@', required: true, multiselect: false, placeholder: '@ConSelB@', optionsName: 'CommonList' },
                                                { id: 'MultiSelect', type: 'toggle', label: '@ConMul@' },
                                                { id: 'ParentList', type: 'parentlinkeditor', label: '@ConParLin@' }
                                            ]
                                        },
                                        {
                                            key: 'fg3a',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '453, 454, 151'}, { effect: 'visible', field: 'MultiSelect', rule: '!=', value: 'Yes'}],
                                            fields: [
                                                { id: 'AllowOther', type: 'toggle', label: '@ConAllowOther@', defaultValue: 'No' }
                                            ]
                                        },
                                        {
                                            key: 'fg3b',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '453, 454, 151'}, { effect: 'visible', field: 'MultiSelect', rule: '!=', value: 'Yes'}, { effect: 'visible', field: 'AllowOther', rule: '=', value: 'Yes'}],
                                            fields: [
                                                { id: 'OtherDetailsLabel', type: 'singleline', label: '@ConOtherPrompt@', placeholder: '@ConEntOtherPrompt@' }
                                            ]
                                        },
                                        {
                                            key: 'fg4',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '458'}],
                                            fields: [
                                                { id: 'Min', type: 'number', label: '@GenMin@', DefaultValue: '0' },
                                                { id: 'Max', type: 'number', label: '@GenMax@', DefaultValue: '1000' },
                                                { id: 'Dpts', type: 'number', label: '@ConDec@', DefaultValue: '0' }
                                            ]
                                        },
                                        {
                                            key: 'fg5',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '455, 456'}],
                                            fields: [
                                                { id: 'defaultToNow', type: 'toggle', label: '@ConDefA@' }
                                            ]
                                        },
                                        {
                                            key: 'fg6',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '457'}],
                                            fields: [
                                                { id: 'ToggleDefault', type: 'combobox', label: '@QuaDef@', required: true, multiselect: false, placeholder: '@ConSelC@', optionsName: 'YesNo' }
                                            ]
                                        },
                                        {
                                            key: 'fg7',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '459'}],
                                            fields: [
                                                { id: 'FieldGrid', type: 'fieldgrid', label: '@ConFie@', AddFormUIEvent: 'addfieldgridcolumnuievent', FormUIEvent: 'editfieldgridcolumnuievent', IncludeGridFormButton: true, GridFields: [
                                                        { Id: 'Id', Type: 'hidden' },
                                                        { Id: 'ColumnDisplay', Type: 'text', GridTitle: '@GenNam@', Width: 'extrawide', FieldFormat: '@GridId@' }
                                                    ]
                                                },
                                                { id: 'IncludeAddButton', type: 'toggle', label: '@ConInc@' },
                                                { id: 'IncludeDeleteButton', type: 'toggle', label: '@ConIncA@' }
                                            ]
                                        },
                                        {
                                            key: 'fg9',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '149'}],
                                            fields: [
                                                { id: 'MultiSelect', type: 'toggle', label: '@ConMul@' },
                                                { id: 'ContentTypeIds', type: 'combobox', label: '@ConConD@', required: true, multiselect: true, placeholder: '@ConSelG@', optionsName: 'ContentTypeList' }
                                            ]
                                        },
                                        {
                                            key: 'fg10',
                                            rules:[{ effect: 'visible', field: 'TypeId', rule: '=', value: '150'}],
                                            fields: [
                                                { id: 'MinYears', type: 'number', label: '@GenMin@' },
                                                { id: 'MaxYears', type: 'number', label: '@GenMax@' }
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
