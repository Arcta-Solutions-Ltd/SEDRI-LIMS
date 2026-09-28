using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditPatientUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editpatientuievent',
                        description: 'Edit patient details',
                        type: 'form',
                        action: 'editpatientform'
                    }";

            return newEvent;
        }
    }
}
