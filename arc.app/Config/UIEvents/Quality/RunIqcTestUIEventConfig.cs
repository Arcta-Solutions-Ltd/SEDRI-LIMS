using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class RunIqcTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'runiqctestuievent',
                description: 'Run IQC Test',
                type: 'form',
                action: 'runiqctestform'
            }";
        }
    }
}
