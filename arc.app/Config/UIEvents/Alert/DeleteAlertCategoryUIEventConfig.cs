using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteAlertCategoryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletealertcategoryuievent',
                        description: 'Delete Alert Category',
                        type: 'form',
                        action: 'deletealertcategoryform'
                    }";

            return newEvent;
        }
    }
}
