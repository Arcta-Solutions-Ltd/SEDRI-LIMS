using arc.app.Common;

namespace arc.app.Config.Events
{
    public class EditCommentEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editcomment', 
                        Description: 'Edit Comment',
                        EventType : 'editdata', 
                        Topic : 'Specimen', 
                        TableName: 'SpecimenComment',
                        Mapping: 'editcommenteventmapper',
                        Display: [
                            { Label: 'commenttypeid', Translation: '@GenComF@', List: 'Yes' },
                            { Label: 'comment', Translation: '@GenComT@', List: 'No' },
                            { Label: 'cannedcomment', Translation: '@GenComM@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' },
                        ]
                    }";
        }
    }
}
