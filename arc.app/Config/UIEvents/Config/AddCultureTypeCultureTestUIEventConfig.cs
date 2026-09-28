using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddCultureTypeCultureTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addculturetypeculturetestuievent',
                        description: 'Add culture type culture test default',
                        type: 'form',
                        action: 'addculturetypeculturetestform'
                    }";

            return newEvent;
        }
    }
}
