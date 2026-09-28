using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteCommentUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletecommentuievent',
                        description: 'Delete Comment',
                        type: 'form',
                        action: 'deletecommentform'
                    }";

            return newEvent;
        }
    }
}
