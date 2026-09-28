using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditRuleCategoryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editrulecategory', 
                        Description: '@RulCatC@',
                        EventType : 'editdata',
                        Topic : 'Coding',
                        Mapping: 'editrulecategorymapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'ResultId', rule: 'required', message: ''},
                            { field: 'Name', rule: 'required', message: ''}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'rulecategoryexists', message: '' }
                        ]
                    }";
        }
    }
}
