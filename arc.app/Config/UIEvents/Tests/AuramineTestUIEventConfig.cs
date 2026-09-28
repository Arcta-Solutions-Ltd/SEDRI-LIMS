using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AuramineTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'auraminetestuievent',
                        description: 'Auramine Test',
                        type: 'form',
                        action: 'auraminetestform'
                    }";

            return newEvent;
        }
    }
}
