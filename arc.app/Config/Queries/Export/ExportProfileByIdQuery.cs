using arc.app.Common;

namespace arc.app.Config.Queries.Export
{
    internal class ExportProfileByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'ExportProfileById',
                        'TableName': 'ExportProfile',
                        'Type': 'Single',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'string' },
                            {'Name': 'Name', 'Type': 'string' }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' }
                        ]
                    }";
        }
    }
}
