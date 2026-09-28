using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddDirectTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'adddirecttestuievent',
                        description: 'Add direct test',
                        type: 'form',
                        action: 'adddirecttestform'
                    }";

            return newEvent;
        }
    }
}
