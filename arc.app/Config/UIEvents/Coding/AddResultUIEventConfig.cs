using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddResultUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addresultuievent',
                        description: 'Add Result',
                        type: 'form',
                        action: 'addresultform'
                    }";

            return newEvent;
        }
    }
}
