using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteCultureTypeCultureTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteculturetypeculturetestuievent',
                        description: 'Delete culture type culture test default',
                        type: 'form',
                        action: 'deleteculturetypeculturetestform'
                    }";

            return newEvent;
        }
    }
}
