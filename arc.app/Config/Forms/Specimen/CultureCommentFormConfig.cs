using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class CultureCommentFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'culturecommentform',
                            formtype: 'singlepage',
                            saveEvent: 'culturecommentevent',
                            recordView: 'cultures',
                            initialQuery: 'culturecommentformquery',
                            suppressRecordView: false,
                            pages: [ 'culturecommentpage' ]
                        }";

            return form;
        }
    }
}
