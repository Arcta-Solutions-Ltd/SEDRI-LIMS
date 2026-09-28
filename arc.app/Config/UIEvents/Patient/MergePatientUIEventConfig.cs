using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class MergePatientUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'mergepatientuievent',
                        description: 'Merge patient',
                        type: 'form',
                        action: 'mergepatientform'
                    }";

            return newEvent;
        }
    }
}
