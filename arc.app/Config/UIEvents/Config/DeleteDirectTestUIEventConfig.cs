using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteDirectTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletedirecttestuievent',
                        description: 'Delete direct test',
                        type: 'form',
                        action: 'deletedirecttestform'
                    }";

            return newEvent;
        }
    }
}
