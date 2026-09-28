using arc.app.Common;

namespace arc.app.Config.Queries.Export
{
    internal class ExportProfileAlreadyExistsForAddQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                'Query': 'exportprofilealreadyexistsforadd',
                'TableName': 'exportprofile',
                'Type': 'count',
                'Fields': [
                        {'Name': 'id', 'Type': 'string' }
                    ],
                'Where' : [
                    {'Field': 'name', 'Comparison': 'equals', 'FieldToMatch': 'name'}
                ]
            }";
        }
    }
}
