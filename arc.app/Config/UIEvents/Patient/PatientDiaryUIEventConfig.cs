using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class PatientDiaryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'patientdiaryuievent',
                        description: 'Patient Diary',
                        type: 'diary',
                        action: 'patientdiary'
                    }";

            return newEvent;
        }
    }
}
