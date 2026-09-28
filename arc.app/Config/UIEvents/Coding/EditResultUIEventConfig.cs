using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditResultUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editresultuievent',
                        description: 'Edit Result',
                        type: 'form',
                        action: 'editresultform'
                    }";

            return newEvent;
        }
    }
}
