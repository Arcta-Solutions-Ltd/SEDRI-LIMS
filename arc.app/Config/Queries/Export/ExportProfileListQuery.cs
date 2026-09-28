using arc.app.Common;

namespace arc.app.Config.Queries.Export
{
    internal class ExportProfileListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'ExportProfileList',
                        'TableName': 'ExportProfile',
                        'Type': 'Select',
                        'Translate': true,
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'Name', 'Type': 'string' },
                            { 'Name': 'Description', 'Type': 'string' },
                            { 'Name': 'ModifiedDate', 'Type': 'datetime'}
                        ],
                        'Orderby': 'modifieddate'
                    }";
        }
    }
}
