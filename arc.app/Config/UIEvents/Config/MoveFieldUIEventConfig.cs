using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// UI event configuration for the Move to Form Group action.
    /// </summary>
    internal class MoveFieldUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'movefielduievent',
                        description: 'Move field to form group',
                        type: 'form',
                        action: 'movefieldform'
                    }";

            return newEvent;
        }
    }
}
