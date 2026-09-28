using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddIqcTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'addiqctestuievent',
                description: 'Add IQC Test UI Event',
                type: 'form',
                action: 'addiqctestform'
            }";
        }
    }
}
