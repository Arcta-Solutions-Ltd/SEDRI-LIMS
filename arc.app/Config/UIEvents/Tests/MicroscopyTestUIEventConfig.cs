using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class MicroscopyTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'microscopytestuievent',
                        description: 'Microscopy Test',
                        type: 'form',
                        action: 'microscopytestform'
                    }";

            return newEvent;
        }
    }
}
