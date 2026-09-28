using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// UI event opening the Neoshield neonatal request form from the patient list, where the baby is
    /// already selected.
    /// </summary>
    internal class CreateNeoshieldSpecimenForPatientUIEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the UI event definition.
        /// </summary>
        /// <returns>A string containing the UI event definition in JSON format.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'createneoshieldspecimenforpatientuievent',
                        description: 'Raise a Neoshield neonatal specimen request',
                        type: 'form',
                        action: 'createneoshieldspecimenforpatientform'
                    }";

            return newEvent;
        }
    }
}
