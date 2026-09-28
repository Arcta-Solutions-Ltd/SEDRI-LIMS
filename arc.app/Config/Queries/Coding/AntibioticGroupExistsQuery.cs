using arc.app.Common;

namespace arc.app.Config.Queries.Coding
{
    internal class AntibioticGroupExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'antibioticgroupexistsquery', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'antibioticgroupexistsmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'ListId', 'Comparison': 'equals' },
                            {'Field': 'Value', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
