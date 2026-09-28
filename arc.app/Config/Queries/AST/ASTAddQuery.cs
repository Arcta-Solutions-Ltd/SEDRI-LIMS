using arc.app.Common;

namespace arc.app.Config.Queries.AST
{
    internal class ASTAddQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'astaddquery', 'Type': 'Special', Tablename: 'AST'}";
        }
    }
}
