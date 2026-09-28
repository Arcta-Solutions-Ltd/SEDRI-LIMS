using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class PatientCommentUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'patientcommentuievent',
                        description: 'Patient Comment',
                        type: 'form',
                        action: 'patientcommentform'
                    }";

            return newEvent;
        }
    }
}
