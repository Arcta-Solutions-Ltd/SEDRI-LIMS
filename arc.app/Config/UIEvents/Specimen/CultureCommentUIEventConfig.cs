using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CultureCommentUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'culturecommentuievent',
                        description: 'Culture Comment',
                        type: 'form',
                        action: 'culturecommentform'
                    }";

            return newEvent;
        }
    }
}
