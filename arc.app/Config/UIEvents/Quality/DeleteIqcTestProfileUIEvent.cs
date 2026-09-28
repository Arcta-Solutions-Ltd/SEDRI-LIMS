using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteIqcTestProfileUIEvent : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'deleteiqctestprofileuievent',
                description: 'Delete IQC Test Profile UI Event',
                type: 'form',
                action: 'deleteiqctestprofileform'
            }";
        }
    }
}
