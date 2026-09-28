using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditCommentForSelectorUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editcommentforselectoruievent',
                        description: 'Edit Comment',
                        type: 'form',
                        action: 'editcommentforselectorform'
                    }";

            return newEvent;
        }
    }
}
