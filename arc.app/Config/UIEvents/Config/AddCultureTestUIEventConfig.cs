using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddCultureTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addculturetestuievent',
                        description: 'Add culture test',
                        type: 'form',
                        action: 'addculturetestform'
                    }";

            return newEvent;
        }
    }
}
