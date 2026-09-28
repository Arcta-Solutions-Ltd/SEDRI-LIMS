using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class RuleCategoryInUseQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'rulecategoryinuse', 'TableName': 'ExpertRule', 'Type': 'Count',
                        'ParameterMapping': 'rulecategoryinusemapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'RuleCategoryId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
