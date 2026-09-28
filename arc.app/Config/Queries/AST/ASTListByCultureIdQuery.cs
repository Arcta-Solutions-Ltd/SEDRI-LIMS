using arc.app.Common;

namespace arc.app.Config.Queries.AST
{
    internal class ASTListByCultureIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                        'Query': 'ASTListByCultureId',
                        'TableName': 'AST',
                        'Type': 'Special'
                    }";
        }
    }
}
