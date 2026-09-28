using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditCultureTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editculturetestuievent',
                        description: 'Edit culture test',
                        type: 'form',
                        action: 'editculturetestform'
                    }";

            return newEvent;
        }
    }
}
