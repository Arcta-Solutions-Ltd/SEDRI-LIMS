using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class MovePatientUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'movepatientuievent',
                        description: 'Move patient',
                        type: 'form',
                        action: 'movepatientform'
                    }";

            return newEvent;
        }
    }
}
