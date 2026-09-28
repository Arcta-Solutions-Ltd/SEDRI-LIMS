using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class TagGraphUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'taggraphuievent',
                        description: 'Tag Graph',
                        type: 'graph',
                        action: 'taggraph'
                    }";

            return newEvent;
        }
    }
}
