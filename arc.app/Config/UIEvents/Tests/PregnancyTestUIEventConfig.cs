using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class PregnancyTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'pregnancytestuievent',
                        description: 'Pregnancy',
                        type: 'form',
                        action: 'pregnancytestform'
                    }";

            return newEvent;
        }
    }
}
