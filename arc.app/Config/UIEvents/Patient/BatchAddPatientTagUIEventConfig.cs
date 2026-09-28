using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// Provides configuration for the 'batchaddpatienttaguievent' UI event.
    /// </summary>
    internal class BatchAddPatientTagUIEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the UI event definition as a JSON string.
        /// </summary>
        /// <returns>A JSON-formatted string defining the UI event.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'batchaddpatienttaguievent',
                        description: 'Batch Add Tag',
                        type: 'form',
                        action: 'batchaddpatienttagform'
                    }";

            return newEvent;
        }
    }
}
