using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class OxidaseTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'oxidasetestuievent',
                        description: 'Oxidase Test',
                        type: 'form',
                        action: 'oxidasetestform'
                    }";

            return newEvent;
        }
    }
}
