using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditCommentFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'editcommentform',
                            formtype: 'singlepage',
                            saveEvent: 'editcomment',
                            recordView: 'specimenrecordview',
                            initialQuery: 'editcommentquery',
                            suppressRecordView: false,
                            pages: [ 'editcommentpage' ] 
                        }";

            return form;
        }
    }
}
