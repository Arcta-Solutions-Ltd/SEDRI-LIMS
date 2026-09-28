using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddIqcTestProfileUIEvent : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'addiqctestprofileuievent',
                description: 'Add IQC Test Profile UI Event',
                type: 'form',
                action: 'addiqctestprofileform'
            }";
        }
    }
}
