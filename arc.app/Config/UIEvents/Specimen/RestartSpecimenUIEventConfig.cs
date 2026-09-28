using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class RestartSpecimenUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'restartspecimenuievent',
                        description: 'Restart Specimen',
                        type: 'form',
                        action: 'restartspecimenform'
                    }";

            return newEvent;
        }
    }
}
