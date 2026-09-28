using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class OrganisationGraphUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'organisationgraphuievent',
                        description: 'Organisation Graph',
                        type: 'graph',
                        action: 'organisationgraph'
                    }";

            return newEvent;
        }
    }
}
