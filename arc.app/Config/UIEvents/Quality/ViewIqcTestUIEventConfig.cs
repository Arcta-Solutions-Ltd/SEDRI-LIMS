using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ViewIqcTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'viewiqctestuievent',
                description: 'View IQC Test UI Event',
                type: 'view-record',
                action: 'iqctests'
            }";
        }
    }
}
