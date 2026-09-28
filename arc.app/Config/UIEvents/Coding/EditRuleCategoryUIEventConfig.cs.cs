using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditRuleCategoryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editrulecategoryuievent',
                        description: 'Edit Expert Rule Category',
                        type: 'form',
                        action: 'editrulecategoryform'
                    }";

            return newEvent;
        }
    }
}
