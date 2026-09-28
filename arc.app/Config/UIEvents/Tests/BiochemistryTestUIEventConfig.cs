using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class BiochemistryTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'biochemistrytestuievent',
                        description: 'Biochemistry Test',
                        type: 'form',
                        action: 'biochemistrytestform'
                    }";

            return newEvent;
        }
    }
}
