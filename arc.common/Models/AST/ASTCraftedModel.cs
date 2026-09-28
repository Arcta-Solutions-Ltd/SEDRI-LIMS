using System.Collections.Generic;

namespace arc.common.Models.AST
{
    public class ASTCraftedModel
    {
        public string Name { get; set; }
        public List<ASTCraftedKeyValueModel> Contents { get; set; }
        
    }
}
