using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditAlertCategoryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editalertcategoryuievent',
                        description: 'Edt Alert Category',
                        type: 'form',
                        action: 'editalertcategoryform'
                    }";

            return newEvent;
        }
    }
}
