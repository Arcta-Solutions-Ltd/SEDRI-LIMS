using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Page definition for editing an existing form group.
    /// Rules (RulesEditor), FieldList (field selector).
    /// Initial data and fieldOptions from formgroupquery.
    /// Form group key is system-managed and not editable.
    /// </summary>
    internal class EditFormGroupPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'editformgrouppage',
                            pageTitle: '@ConEdiFG@',
                            text: '',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Rules', type: 'ruleseditor', label: '@GenRul@', context: 'formgroup' },
                                                { id: 'FieldList', type: 'fieldselector', label: '@GenFie@', CanMoveEntries: true, Draggable: true, IncludeOptions: false }
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
