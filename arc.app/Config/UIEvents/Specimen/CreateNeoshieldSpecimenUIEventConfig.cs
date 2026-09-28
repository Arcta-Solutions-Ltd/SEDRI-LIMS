using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// UI event opening the Neoshield neonatal request form from the specimen list.
    /// </summary>
    internal class CreateNeoshieldSpecimenUIEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the UI event definition.
        /// </summary>
        /// <returns>A string containing the UI event definition in JSON format.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'createneoshieldspecimen',
                        description: 'Raise a Neoshield neonatal specimen request',
                        type: 'form',
                        action: 'createneoshieldspecimenform'
                    }";

            return newEvent;
        }
    }
}
