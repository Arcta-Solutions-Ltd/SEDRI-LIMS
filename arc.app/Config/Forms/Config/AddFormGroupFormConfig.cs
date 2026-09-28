using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form definition for adding a new form group to a page.
    /// Uses formfieldoptionsquery for rule field options.
    /// </summary>
    internal class AddFormGroupFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addformgroupform',
                        viewTitle: '@ConAddFG@',
                        saveEvent: 'addformgroup',
                        initialquery: 'formfieldoptionsquery',
                        suppressRecordView: true,
                        pages: [ 'addformgrouppage']
                    }";

            return form;
        }
    }
}
