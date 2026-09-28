using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteTableUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletetableuievent',
                        description: 'Delete Table',
                        type: 'form',
                        action: 'deletetableform'
                    }";

            return newEvent;
        }
    }
}
