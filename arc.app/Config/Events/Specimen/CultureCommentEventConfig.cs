using arc.app.Common;

namespace arc.app.Config.Events
{
    public class CultureCommentEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'culturecommentevent', 
                        Description: '@GenComU@',
                        EventType : 'adddata', 
                        Topic : 'Culture', 
                        TableName: 'SpecimenComment',
                        RequiresSpecimenWorkflow: false,
                        Mapping: 'culturecommentmapper',
                        ValidationRules: [
                            { field: 'commenttype', rule: 'required', message: '@SpeB@'}
                        ],
                        Display: [
                            { Label: 'commenttype', Translation: 'Comment Type', List: 'Yes' },
                            { Label: 'cannedcomment', Translation: 'Canned Comment', List: 'Yes' },
                            { Label: 'comment', Translation: '@GenComT@', List: 'No' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' },
                        ]
                    }";
        }
    }
}
