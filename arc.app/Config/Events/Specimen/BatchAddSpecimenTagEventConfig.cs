using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for batch add specimen tag.
    /// Runs addspecimentag for each selected specimen.
    /// </summary>
    internal class BatchAddSpecimenTagEventConfig : IDefinition
    {
        /// <summary>
        /// Gets the event configuration as a JSON string.
        /// </summary>
        /// <returns>A JSON string representing the event configuration.</returns>
        public string Get()
        {
            return @"{
                    EventName: 'batchaddspecimentag',
                    Description: '@SpeBatI@',
                    EventType: 'batch',
                    Topic: 'Tags',
                    TableName: 'Specimen',
                    BatchEvent: 'addspecimentag'
                }";
        }
    }
}
