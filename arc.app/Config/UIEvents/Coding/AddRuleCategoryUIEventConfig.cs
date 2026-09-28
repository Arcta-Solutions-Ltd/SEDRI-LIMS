using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddRuleCategoryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addrulecategoryuievent',
                        description: 'Add Expert Rule Category',
                        type: 'form',
                        action: 'addrulecategoryform'
                    }";

            return newEvent;
        }
    }
}
