using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for batch add patient tag.
    /// Runs addpatienttag for each selected patient.
    /// </summary>
    internal class BatchAddPatientTagEventConfig : IDefinition
    {
        /// <summary>
        /// Gets the event configuration as a JSON string.
        /// </summary>
        /// <returns>A JSON string representing the event configuration.</returns>
        public string Get()
        {
            return @"{
                    EventName: 'batchaddpatienttag',
                    Description: '@SpeBatI@',
                    EventType: 'batch',
                    Topic: 'Tags',
                    TableName: 'Patient',
                    RequiresSpecimenWorkflow: false,
                    BatchEvent: 'addpatienttag'
                }";
        }
    }
}
