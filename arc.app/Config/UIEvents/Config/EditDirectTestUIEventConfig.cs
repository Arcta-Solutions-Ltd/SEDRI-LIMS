using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditDirectTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editdirecttestuievent',
                        description: 'Edit direct test',
                        type: 'form',
                        action: 'editdirecttestform'
                    }";

            return newEvent;
        }
    }
}
