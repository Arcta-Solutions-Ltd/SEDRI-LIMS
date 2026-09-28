using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Represents the configuration for the View Specimen Record event.
    /// This is a synthetic event used for permissioning the viewspecimenrecord UI event.
    /// </summary>
    internal class ViewSpecimenRecordEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the configuration for the View Specimen Record event in JSON format.
        /// </summary>
        /// <returns>A JSON string containing the event configuration.</returns>
        public string Get()
        {
            return @"{
                    EventName: 'viewspecimenrecord',
                    Description: '@SpeVieA@',
                    EventType: 'special',
                    Topic: 'Specimen',
                    TableName: 'Specimen'
                }";
        }
    }
}
