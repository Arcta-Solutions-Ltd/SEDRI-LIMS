using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteCommentFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'deletecommentform',
                            saveEvent: 'deletecomment',
                            formtype: 'singlepage',
                            recordView: 'specimenrecordview',
                            initialQuery: 'deletecommentquery',
                            suppressRecordView: false,
                            pages: ['deletecommentpage']
                        }";

            return form;
        }
    }
}
