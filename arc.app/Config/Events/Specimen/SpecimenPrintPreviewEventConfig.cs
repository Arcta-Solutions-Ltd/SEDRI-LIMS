using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Represents the configuration for the Specimen Print Preview event.
    /// </summary>
    internal class SpecimenPrintPreviewEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the configuration for the Specimen Print Preview event in JSON format.
        /// </summary>
        /// <returns>A JSON string containing the event configuration.</returns>
        public string Get()
        {
            return @"{
                    EventName: 'SpecimenPrintPreview',
                    Description: '@RepSpeA@',
                    EventType : 'special',
                    Topic : 'Specimen',
                    TableName: 'Specimen'
                }";
        }
    }
}
