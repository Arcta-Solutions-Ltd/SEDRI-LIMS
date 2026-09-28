using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class SynonymUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'synonymuievent',
                        description: 'Manage Synonyms',
                        type: 'form',
                        action: 'synonymform'
                    }";

            return newEvent;
        }
    }
}
