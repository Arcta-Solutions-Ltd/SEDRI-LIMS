using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteListUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletelistuievent',
                        description: 'Delete List',
                        type: 'form',
                        action: 'deletelistform'
                    }";

            return newEvent;
        }
    }
}
