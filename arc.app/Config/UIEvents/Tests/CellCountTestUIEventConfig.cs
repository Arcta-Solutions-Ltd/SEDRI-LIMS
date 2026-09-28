using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CellCountTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'cellcounttestuievent',
                        description: 'Cell Count Test',
                        type: 'form',
                        action: 'cellcounttestform'
                    }";

            return newEvent;
        }
    }
}
