using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// UI event configuration for the Move Subsection action.
    /// </summary>
    internal class MoveFormGroupUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'moveformgroupuievent',
                        description: 'Move subsection to another page',
                        type: 'form',
                        action: 'moveformgroupform'
                    }";

            return newEvent;
        }
    }
}
