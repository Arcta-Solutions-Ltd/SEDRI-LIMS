using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ASTUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'ast',
                        description: 'Add or edit AST results for an organism',
                        type: 'form',
                        action: 'ast'
                    }";

            return newEvent;
        }
    }
}
