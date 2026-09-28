using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteRuleCategoryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleterulecategoryuievent',
                        description: 'Delete Expert Rule Category',
                        type: 'form',
                        action: 'deleterulecategoryform'
                    }";

            return newEvent;
        }
    }
}
