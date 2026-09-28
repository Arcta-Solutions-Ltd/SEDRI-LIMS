using arc.app.Common;

namespace arc.app.Config.Queries
{
    public class ConfigHistoryListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'ConfigHistoryListQuery',
                        'TableName': 'configshistory',
                        'Type': 'Select',
                        'Fields': [
                            { 'Name': 'id', 'Type': 'string' },
                            { 'Name': 'LastModifiedDate', 'Type': 'datetime'}
                        ],
                        'Joins': [
                            { 'Table': 'Configs', 'Fields': [{'Name': 'ConfigName'}] }
                        ],
                        'Orderby': 'lastmodifieddate',
                        'Descending': true
                    }";
        }

    }
}
