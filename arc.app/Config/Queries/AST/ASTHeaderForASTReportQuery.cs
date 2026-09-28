using arc.app.Common;

namespace arc.app.Config.Queries.AST
{
    internal class ASTHeaderForASTReportQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                        'Query': 'ASTHeaderForASTReport',
                        'TableName': 'AST',
                        'Type': 'Single',
                        'ResultMapping': 'astreportheadermapper',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'Int' }
                        ]
                    }";
        }
    }
}
