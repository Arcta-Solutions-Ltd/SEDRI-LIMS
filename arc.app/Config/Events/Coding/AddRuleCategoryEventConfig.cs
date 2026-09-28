using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddRuleCategoryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addrulecategory', 
                        Description: '@RulCatB@',
                        EventType : 'adddata', 
                        Topic : 'Coding',
                        Mapping: 'addrulecategorymapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: ''}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'rulecategoryexists', message: '' }
                        ]
                    }";
        }
    }
}
