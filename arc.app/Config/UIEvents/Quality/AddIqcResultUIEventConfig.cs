using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddIqcResultUIEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'addiqcresultuievent',
                description: 'Add IQC Result UI Event',
                type: 'form',
                action: 'addiqcresultform'
            }";
        }
    }
}
