using System.Collections.Generic;

namespace arc.common.Models.AST
{
    public class ASTCraftedKeyValueModel
    {
        public string Key { get; set; }
        public ASTUpdateEventModel Value { get; set; }
    }
}
