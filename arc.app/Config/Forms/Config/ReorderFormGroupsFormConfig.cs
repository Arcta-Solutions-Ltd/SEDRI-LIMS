using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form definition for reordering form groups on a page.
    /// Uses formgroupsforpagequery for initial data.
    /// </summary>
    internal class ReorderFormGroupsFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'reorderformgroupsform',
                        viewTitle: '@ConReorderFG@',
                        saveEvent: 'reorderformgroups',
                        initialquery: 'formgroupsforpagequery',
                        suppressRecordView: true,
                        pages: [ 'reorderformgroupspage']
                    }";

            return form;
        }
    }
}
