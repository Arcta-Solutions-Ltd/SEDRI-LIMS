using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteTestMethodUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletetestmethoduievent',
                        description: 'Delete Test Method',
                        type: 'form',
                        action: 'deletetestmethodform'
                    }";

            return newEvent;
        }
    }
}
