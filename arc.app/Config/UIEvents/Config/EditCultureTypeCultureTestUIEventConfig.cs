using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditCultureTypeCultureTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editculturetypeculturetestuievent',
                        description: 'Edit culture type culture test default',
                        type: 'form',
                        action: 'editculturetypeculturetestform'
                    }";

            return newEvent;
        }
    }
}
