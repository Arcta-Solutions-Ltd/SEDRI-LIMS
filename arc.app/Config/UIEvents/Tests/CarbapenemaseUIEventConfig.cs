using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CarbapenemaseUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'carbapenemasetestuievent',
                        description: 'Carbapenemase Test',
                        type: 'form',
                        action: 'carbapenemasetestform'
                    }";

            return newEvent;
        }
    }
}
