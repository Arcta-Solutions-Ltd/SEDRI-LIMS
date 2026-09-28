using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form definition for editing an existing form group.
    /// Uses formgroupquery for initial data including fieldOptions for RulesEditor.
    /// </summary>
    internal class EditFormGroupFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editformgroupform',
                        viewTitle: '@ConEdiFG@',
                        saveEvent: 'editformgroup',
                        initialquery: 'formgroupquery',
                        suppressRecordView: true,
                        pages: [ 'editformgrouppage']
                    }";

            return form;
        }
    }
}
