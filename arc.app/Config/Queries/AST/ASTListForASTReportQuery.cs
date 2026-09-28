using arc.app.Common;

namespace arc.app.Config.Queries.AST
{
    internal class ASTListForASTReportQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                        'Query': 'ASTListForASTReport',
                        'TableName': 'AST',
                        'Type': 'Select',
                        'ResultMapping': 'astreportmapper',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'Int' },
                            { 'Name': 'CultureId', 'Type': 'Int' },
                            { 'Name': 'TestType', 'Type': 'string' },
                            { 'Name': 'AdditionalNotes', 'Type': 'string'}
                        ],
                        'ListItems': 'Antibiotic, Susceptibility',
                        'Where' : [
                            {'Field': 'CultureId', 'Comparison': '=' }
                        ],
                        'OrderBy' : 'Id'
                    }";
        }
    }
}
