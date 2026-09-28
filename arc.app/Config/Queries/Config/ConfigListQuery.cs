using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ConfigListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'ConfigList',
                        'TableName': 'Configs',
                        'Type': 'Select',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'ConfigName', 'Type': 'string' },
                            { 'Name': 'Grouping', 'Type': 'string' },
                            { 'Name': 'lastmodifieddate', 'Type': 'datetime'}
                        ],
                        'Where' : [
                            {'Field': 'Grouping', 'Comparison': 'equals' },
                        ],
                        'Orderby': 'lastmodifieddate'
                    }";
        }
    }
}
