using arc.app.Common;

namespace arc.app.Config.Queries.AST
{
    internal class ASTListWideQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'astlistwide', 'Type': 'Special', Tablename: 'AST', 'ResultMapping': 'whonetexportmapper'}";
        }
    }
}
