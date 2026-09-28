using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// Provides configuration for the 'batchaddspecimentaguievent' UI event.
    /// </summary>
    internal class BatchAddSpecimenTagUIEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the UI event definition as a JSON string.
        /// </summary>
        /// <returns>A JSON-formatted string defining the UI event.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'batchaddspecimentaguievent',
                        description: 'Batch Add Tag',
                        type: 'form',
                        action: 'batchaddspecimentagform'
                    }";

            return newEvent;
        }
    }
}
