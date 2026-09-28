using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteCultureTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteculturetestuievent',
                        description: 'Delete culture test',
                        type: 'form',
                        action: 'deleteculturetestform'
                    }";

            return newEvent;
        }
    }
}
