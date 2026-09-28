using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditSpecimenUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editspecimenuievent',
                        description: 'Edit specimen',
                        type: 'form',
                        action: 'editspecimenform'
                    }";

            return newEvent;
        }
    }
}
