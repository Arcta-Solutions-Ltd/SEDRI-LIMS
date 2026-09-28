using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditCommentUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editcommentuievent',
                        description: 'Edit Comment',
                        type: 'form',
                        action: 'editcommentform'
                    }";

            return newEvent;
        }
    }
}
