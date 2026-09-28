using arc.app.Common;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Form definition for deleting a form group.
    /// </summary>
    internal class DeleteFormGroupFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleteformgroupform',
                        viewTitle: '@ConDelFG@',
                        saveEvent: 'deleteformgroup',
                        initialquery: 'formgroupquery',
                        suppressRecordView: true,
                        pages: [ 'deleteformgrouppage']
                    }";

            return form;
        }
    }
}
