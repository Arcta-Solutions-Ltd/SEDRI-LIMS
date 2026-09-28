using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form definition for moving a form group (subsection) to another page within the same form.
    /// </summary>
    internal class MoveFormGroupFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'moveformgroupform',
                        viewTitle: '@ConMovFGGrp@',
                        saveEvent: 'moveformgroup',
                        initialquery: 'moveformgroupquery',
                        suppressRecordView: true,
                        pages: [ 'moveformgrouppage']
                    }";

            return form;
        }
    }
}
