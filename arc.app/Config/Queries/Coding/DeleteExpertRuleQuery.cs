using arc.app.Common;

namespace arc.app.Config.Queries.Coding;
internal class DeleteExpertRuleQuery : IDefinition
{
    public string Get()
    {
        return @"{  
                        'Query': 'deleteexpertrule', 'TableName': 'ExpertRule', 'Type': 'Single',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'ExpertRuleName', 'Type': 'string'},
                            {'Name': 'RuleText', 'Type': 'string'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': 'equals' }
                        ]
                    }";
    }
}
