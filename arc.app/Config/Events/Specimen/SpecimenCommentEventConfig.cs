using arc.app.Common;

namespace arc.app.Config.Events
{
    public class SpecimenCommentEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'specimencomment', 
                        Description: '@SpeEnt@',
                        EventType : 'adddata', 
                        Topic : 'Specimen', 
                        TableName: 'SpecimenComment',
                        Mapping: 'specimencommentmapper',
                        ValidationRules: [
                            { field: 'commenttype', rule: 'required', message: '@SpeB@'}
                        ],
                        Display: [
                            { Label: 'commenttype', Translation: '@GenComF@', List: 'Yes' },
                            { Label: 'cannedcomment', Translation: '@GenComM@', List: 'Yes' },
                            { Label: 'comment', Translation: '@GenComT@', List: 'No' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' },
                        ]
                    }";
        }
    }
}
