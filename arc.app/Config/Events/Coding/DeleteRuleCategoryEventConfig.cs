using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteRuleCategoryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleterulecategory', 
                        Description: '@RulCatD@',
                        EventType : 'deletedata', 
                        Topic : 'Coding',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: ''}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'rulecategoryinuse', message: '' }
                        ]
                    }";
        }
    }
}
