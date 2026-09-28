using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// This class defines the event configuration for batch specimen approval at level one.
    /// </summary>
    internal class BatchSpecimenApprovalOneEventConfig : IDefinition
    {
        /// <summary>
        /// Gets the event configuration as a JSON string.
        /// </summary>
        /// <returns>A JSON string representing the event configuration.</returns>
        public string Get()
        {
            return @"{ 
                    EventName: 'batchspecimenapprovalone', 
                    Description: '@SpeBatB@',
                    EventType: 'batch', 
                    Topic : 'Specimen', 
                    TableName: 'Specimen',
                    BatchEvent: 'specimenapprovalone'
                }";
        }
    }
}
