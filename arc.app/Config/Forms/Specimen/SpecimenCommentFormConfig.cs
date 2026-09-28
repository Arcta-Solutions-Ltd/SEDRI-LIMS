using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class SpecimenCommentFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'specimencommentform',
                            formtype: 'singlepage',
                            saveEvent: 'specimencomment',
                            recordView: 'specimenrecordview',
                            suppressRecordView: false,
                            pages: [ 'specimencomment' ]
                        }";

            return form;
        }
    }
}
