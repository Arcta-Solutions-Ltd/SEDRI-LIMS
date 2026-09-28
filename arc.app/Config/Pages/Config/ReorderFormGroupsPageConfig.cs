using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Page definition for reordering form groups on a page.
    /// FieldList (field selector with CanMoveEntries) shows form groups in current order.
    /// </summary>
    internal class ReorderFormGroupsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'reorderformgroupspage',
                            pageTitle: '@ConReorderFG@',
                            text: '',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'FieldList', type: 'fieldselector', label: '@ConFG@', CanMoveEntries: true, Draggable: true, IncludeOptions: false }
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
