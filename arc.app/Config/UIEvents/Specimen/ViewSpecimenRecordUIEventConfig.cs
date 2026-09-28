using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    /// <summary>
    /// This class represents the configuration for the "View Specimen Record" UI event.
    /// </summary>
    internal class ViewSpecimenRecordUIEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the configuration for the "View Specimen Record" UI event.
        /// </summary>
        /// <returns>A string containing the UI event configuration in JSON format.</returns>
        public string Get()
        {
            var newEvent = @"{
                        name: 'viewspecimenrecord',
                        description: 'View specimen record',
                        type: 'view-record',
                        action: 'specimenrecordview'
                    }";

            return newEvent;
        }
    }
}

