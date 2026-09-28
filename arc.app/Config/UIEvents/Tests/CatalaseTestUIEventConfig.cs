using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CatalaseTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'catalasetestuievent',
                        description: 'Catalase Test',
                        type: 'form',
                        action: 'catalasetestform'
                    }";

            return newEvent;
        }
    }
}
