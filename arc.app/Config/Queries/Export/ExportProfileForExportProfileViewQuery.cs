using arc.app.Common;

namespace arc.app.Config.Queries.Export
{
    internal class ExportProfileForExportProfileViewQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'exportprofileview',
                        'TableName': 'ExportProfile',
                        'Type': 'Single',
                        'Translate': true,
                        'ResultMapping': 'exportprofileviewmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'string' },
                            {'Name': 'Name', 'Type': 'string' },
                            {'Name': 'Description', 'Type': 'string' },
                            {'Name': 'ModifiedDate', 'Type':'datetime'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' }
                        ]
                    }";
        }
    }
}
