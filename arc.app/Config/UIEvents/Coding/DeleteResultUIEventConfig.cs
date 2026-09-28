using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteResultUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteresultuievent',
                        description: 'Delete Result',
                        type: 'form',
                        action: 'deleteresultform'
                    }";

            return newEvent;
        }
    }
}
