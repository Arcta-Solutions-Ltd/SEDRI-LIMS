using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditLaboratoryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editlaboratoryuievent',
                        description: 'Edit Laboratory',
                        type: 'form',
                        action: 'editlaboratoryform'
                    }";

            return newEvent;
        }
    }
}
