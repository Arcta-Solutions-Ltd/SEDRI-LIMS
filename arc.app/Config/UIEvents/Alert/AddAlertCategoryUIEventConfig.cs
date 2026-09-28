using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddAlertCategoryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addalertcategoryuievent',
                        description: 'Add Alert Category',
                        type: 'form',
                        action: 'addalertcategoryform'
                    }";

            return newEvent;
        }
    }
}
