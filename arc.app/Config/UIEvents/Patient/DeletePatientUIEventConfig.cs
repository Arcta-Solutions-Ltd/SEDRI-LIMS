using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeletePatientUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletepatientuievent',
                        description: 'Delete patient',
                        type: 'form',
                        action: 'deletepatientform'
                    }";

            return newEvent;
        }
    }
}
