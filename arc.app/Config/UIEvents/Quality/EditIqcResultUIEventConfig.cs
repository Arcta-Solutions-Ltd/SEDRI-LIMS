using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditIqcResultUIEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'editiqcresultuievent',
                description: 'Edit IQC result',
                type: 'form',
                action: 'editiqcresultform'
            }";
        }
    }
}
